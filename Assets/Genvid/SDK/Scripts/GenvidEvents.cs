using System;
using System.Collections.Generic;
using GenvidSDKCSharp;
using UnityEngine;
using UnityEngine.Events;
using Genvid;

namespace Genvid
{
    /// <summary>
    /// Class representing a Genvid event as a Unity event.
    /// </summary>
    [Serializable]
    public class GenvidEventType : UnityEvent<string, GenvidSDK.EventResult[], int, IntPtr>
    {
    }

    /// <summary>
    /// Class representing a Genvid event.
    /// </summary>
    [Serializable]
    public class GenvidEventElement
    {
        public string Id;
        public GenvidEventType OnEventTriggered;
    }

    namespace Plugin
    {
        namespace Channel
        {
            /// <summary>
            /// Specializes the Genvid checker for Genvid events.
            /// </summary>
            [Serializable]
            public class Events : GenvidChecker<GenvidSDK.EventSummary, GenvidEventParameters>
            {
                /// <summary>
                /// List of event parameters.
                /// </summary>
                public List<GenvidEventParameters> Settings;

#if !(UNITY_EDITOR || UNITY_STANDALONE_WIN)
            // Disable warning for other platforms.
#pragma warning disable 414
#endif
                /// <summary>
                /// Defines what to do when a Genvid event is received.
                /// </summary>
                private GenvidSDK.EventSummaryCallback m_EventCallback = null;

#if !(UNITY_EDITOR || UNITY_STANDALONE_WIN)
#pragma warning restore 414
#endif
                /// <summary>
                /// The string name of this class. Used in logs.
                /// </summary>
                protected override string Typename { get { return "Event"; } }

                /// <summary>
                /// Used to invoke the Genvid event-reception callback when a new event is processed by the checker.
                /// </summary>
                /// <param name="dataEvent">Container with the parameters and received data.</param>
                protected override void OnInvokeFunction(DataFunction<GenvidEventParameters, GenvidSDK.EventSummary> dataEvent)
                {
                    dataEvent.@event.Summary = dataEvent.Data;
                    dataEvent.@event.UserData = dataEvent.UserData;
                    dataEvent.@event.OnEventReceived();
                }

                /// <summary>
                /// Initializes the parameter list for the GenvidChecker.
                /// Called before the checker iterates on the parameters.
                /// </summary>
                protected override void Init()
                {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                    m_EventCallback = new GenvidSDK.EventSummaryCallback(EventCallbackFunction);
                    m_SubscribedIds = Settings;
#endif
                }

                /// <summary>
                /// Cleans up Genvid event-specific members.
                /// Done after the checker is done unsubscribing the Genvid events.
                /// </summary>
                protected override void Term()
                {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                    m_EventCallback = null;
                    m_SubscribedIds = null;
#endif
                }

                /// <summary>
                /// Subscribes a Genvid event.
                /// </summary>
                /// <param name="id">ID of the Genvid event.</param>
                /// <param name="userData">The received data concerning the Genvid event.</param>
                /// <returns>The operation status. GenvidSDK.success if everything went well.</returns>
                protected override GenvidSDK.Status SubscribeImpl(string id, IntPtr userData)
                {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                    return GenvidSDK.Subscribe(id, m_EventCallback, userData);
#else
                return GenvidSDK.Status.Success;
#endif
                }

                /// <summary>
                /// Unsubscribes a Genvid event.
                /// </summary>
                /// <param name="id">ID of the Genvid event.</param>
                /// <param name="userData">The received data concerning the Genvid event.</param>
                /// <returns>The operation status. GenvidSDK.success if everything went well.</returns>
                protected override GenvidSDK.Status UnsubscribeImpl(string id, IntPtr userData)
                {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                    return GenvidSDK.Unsubscribe(id, m_EventCallback, userData);
#else
                return GenvidSDK.Status.Success;
#endif
                }

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                /// <summary>
                /// Add an event to the list of received events.
                /// </summary>
                /// <param name="result">The event result.</param>
                /// <param name="userData">The received data concerning the event.</param>
                private void EventCallbackFunction(IntPtr summaryData, IntPtr userData)
                {
                    try
                    {
                        var summary = GenvidSDK.GetSummary(summaryData);
                        PushData(summary.id, summary, userData);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Exception during Event Callback: " + e.ToString());
                    }
                }
#endif
            }
        }
    }
}