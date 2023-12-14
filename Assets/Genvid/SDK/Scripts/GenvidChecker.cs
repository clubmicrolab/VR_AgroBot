using UnityEngine;
using System;
using GenvidSDKCSharp;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Genvid
{
    /// <summary>
    /// Helper encapsulating an object requiring an ID and an event callback.
    /// </summary>
    [Serializable]
    public class BaseEventElement<T> where T : UnityEventBase
    {
        public string Id;
        public T OnEventTriggered;
    }

    /// <summary>
    /// Helper encapsulating the data and parameters.
    /// </summary>
    [Serializable]
    public class DataFunction<T0, T1>
    {
        public T0 @event;
        public T1 Data;
        public IntPtr UserData;
    }

    namespace Plugin
    {

        /// <summary>
        /// Class handling the mass subscription to events, treatment of events, and removal of event subscriptions.
        /// Base classes need to define type-specific implementations.
        /// </summary>
        [Serializable]
        public abstract class GenvidChecker<T1, T2> : IGenvidPlugin
            where T1 : class                     // GenvidSDK class
            where T2 : AGenvidParametersBase     // Parameters
        {
            /// <summary>
            /// List of parameters.
            /// </summary> 
            protected List<T2> m_SubscribedIds;

            /// <summary>
            /// Toggled on when the event and data pools are initialized.
            /// </summary> 
            public bool IsInitialized { get; private set; }

            /// <summary>
            /// Specifies logging verbosity. Toggle on for more logging info.
            /// </summary>
            public bool VerboseLog { get; set; }

#if !(UNITY_EDITOR || UNITY_STANDALONE_WIN)
            // Disable warning for other platforms.
#pragma warning disable 414
#endif
            /// <summary>
            /// Event definition cache. Stores event parameters
            /// based on the event ID (defined by the user in the parameters).
            /// </summary>
            private Dictionary<string, T2> m_EventData2 = null;

            /// <summary>
            /// Collction of received events.
            /// </summary>
            private Stack<DataFunction<T2, T1>> m_EventPool2 = null;

#if !(UNITY_EDITOR || UNITY_STANDALONE_WIN)
#pragma warning restore 414
#endif

            /// <summary>
            /// Name of the event type. (Event, Command, etc.)
            /// </summary>
            protected abstract string Typename { get; }

            /// <summary>
            /// Base classes add initialization functionality here.
            /// </summary>
            protected abstract void Init();

            /// <summary>
            /// Base classes add cleanup functionality here.
            /// </summary>
            protected abstract void Term();

            /// <summary>
            /// Base classes need to define their subscription. (SubscribeEvent, SubscribeCommand, etc.)
            /// </summary>
            /// <param name="id">ID of the event.</param>
            /// <param name="userData">The received data concerning the event.</param>
            /// <returns>The operation status. GenvidSDK.success if everything went well.</returns>
            protected abstract GenvidSDK.Status SubscribeImpl(string id, IntPtr userData);

            /// <summary>
            /// Base classes need to define their subscription removal.(UnsubscribeEvent, UnsubscribeCommand, etc.)
            /// </summary>
            /// <param name="id">ID of the event.</param>
            /// <param name="userData">The received data concerning the event.</param>
            /// <returns>The operation status. GenvidSDK.success if everything went well.</returns>
            protected abstract GenvidSDK.Status UnsubscribeImpl(string id, IntPtr userData);

            /// <summary>
            /// Base classes need to define what to do with a received event.
            /// </summary>
            /// <param name="id">ID of the event.</param>
            /// <param name="data">Container with the parameters and received data.</param>
            protected abstract void OnInvokeFunction(DataFunction<T2, T1> data);

            /// <summary>
            /// Declare a subscription to an event triggered outside the game.
            /// </summary>
            /// <param name="id">ID of the event.</param>
            /// <param name="userData">The received data concerning the event.</param>
            /// <returns>True if the subscription is successfully created, false otherwise.</returns>
            private bool Subscribe(string id, IntPtr userData)
            {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                var status = SubscribeImpl(id, userData);
                if (GenvidSDK.StatusFailed(status))
                {
                    Debug.LogError("[" + Typename + "] Error while Subscribing the '" + id + "', status: " + GenvidSDK.StatusToString(status));
                    return false;
                }

                Debug.Log("[" + Typename + "] '" + id + "' subscribed successfully.");
#endif
                return true;
            }

            /// <summary>
            /// Unsubscribe to an event triggered outside the game.
            /// </summary>
            /// <param name="id">ID of the event.</param>
            /// <param name="userData">The received data concerning the event.</param>
            /// <returns>True if the subscription is successfully removed, false otherwise.</returns>
            private bool Unsubscribe(string id, IntPtr userData)
            {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                var status = UnsubscribeImpl(id, userData);
                if (GenvidSDK.StatusFailed(status))
                {
                    Debug.LogError("[" + Typename + "] Error while unsubscribing '" + id + "', status: " + GenvidSDK.StatusToString(status));
                    return false;
                }

                Debug.Log("[" + Typename + "] '" + id + "' successfully unsubscribed.");
#endif
                return true;
            }

            /// <summary>
            /// Create subscriptions based on user-defined parameters.
            /// </summary>
            /// <returns>True if all subscriptions are created successfully, false otherwise.</returns>
            public bool Initialize()
            {
                bool result = true;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                if (!IsInitialized)
                {
                    Init();

                    m_EventData2 = new Dictionary<string, T2>();
                    m_EventPool2 = new Stack<DataFunction<T2, T1>>();

                    int index = 0;
                    foreach (var ev in m_SubscribedIds)
                    {
                        var userData = new IntPtr(index);
                        if (Subscribe(ev.Id, userData))
                        {
                            ev.UserData = userData;
                            m_EventData2.Add(ev.Id, ev);
                            ++index;
                        }
                        else
                        {
                            result = false;
                        }
                    }
                }

                IsInitialized = true;
#endif
                return result;
            }

            /// <summary>
            /// Unsubscribe all saved subscriptions.
            /// </summary>
            /// <returns>True if all subscriptions are removed successfully, false otherwise.</returns>
            public bool Terminate()
            {
                bool result = true;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                if (IsInitialized)
                {
                    if (m_EventData2 != null)
                    {
                        foreach (var eventName in m_EventData2)
                        {
                            result &= Unsubscribe(eventName.Key, eventName.Value.UserData);
                        }

                        // New
                        m_EventData2.Clear();
                        m_EventPool2.Clear();
                        m_EventData2 = null;
                        m_EventPool2 = null;
                    }

                    Term();
                    IsInitialized = false;
                }
#endif
                return result;
            }

            /// <summary>
            /// Add an event to the list of received events.
            /// </summary>
            /// <param name="id">ID of the event.</param>
            /// <param name="data">The event data. Usually the event result or definition.</param>
            /// <param name="userData">The received data concerning the event.</param>
            protected void PushData(string id, T1 data, IntPtr userData)
            {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                if (m_EventPool2 == null)
                {
                    Debug.Log("[" + Typename + "] '" + id + "' has been ignored because the Pool is null.");
                }

                T2 eventParams;
                if (m_EventData2.TryGetValue(id, out eventParams))
                {
                    if (eventParams.HasRegisteredListeners)
                    {
                        try
                        {
                            var dataEvent = new DataFunction<T2, T1>();
                            dataEvent.Data = data;
                            dataEvent.@event = eventParams;
                            dataEvent.UserData = userData;
                            m_EventPool2.Push(dataEvent);
                        }
                        catch (OutOfMemoryException ex)
                        {
                            Debug.LogError(ex.Message);
                        }
                    }
                }
#endif
            }

            /// <summary>
            /// Defined what to do on object startup.
            /// </summary>
            public void Start()
            {
                /* nothing to do */
            }

            /// <summary>
            /// Iterates through the list of received events and applies the base-class defined
            /// functionality with those events.
            /// </summary>
            public void Update()
            {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                if (IsInitialized)
                {
                    if (m_EventPool2 != null)
                    {
                        while (m_EventPool2.Count > 0)
                        {
                            var dataEvent = m_EventPool2.Pop();
                            OnInvokeFunction(dataEvent);
                        }
                    }
                }
#endif
            }
        }
    }
}