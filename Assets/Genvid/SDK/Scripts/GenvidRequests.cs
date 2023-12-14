using System;
using System.Collections.Generic;
using Genvid;
using GenvidSDKCSharp;
using UnityEngine;
using UnityEngine.Events;

namespace Genvid
{
    /// <summary>
    /// Class representing a request event as a Unity event.
    /// The first parameter is the topic, the second is the result, and the last one is the
    /// user data.
    /// </summary>
    [Serializable]
    public class RequestEvent : UnityEvent<string, GenvidSDK.RequestResult, IntPtr> { }

    /// <summary>
    /// Class representing a request.
    /// </summary>
    [Serializable]
    public class RequestElement
    {
        public string Topic;
        public RequestEvent OnRequestTriggered;
    }

    namespace Plugin
    {
        namespace Channel
        {
            /// <summary>
            /// Specializes the Genvid checker for requests.
            /// </summary>
            [Serializable]
            public class Requests : GenvidChecker<GenvidSDK.RequestResult, GenvidRequestParameters>
            {
                /// <summary>
                /// List of request parameters.
                /// </summary>
                public List<GenvidRequestParameters> Settings;

#if !(UNITY_EDITOR || UNITY_STANDALONE_WIN)
            // Disable warning for other platforms.
#pragma warning disable 414
#endif
                /// <summary>
                /// Defines what to do when a request is received.
                /// </summary>
                private GenvidSDK.RequestCallback m_RequestCallback = null;

#if !(UNITY_EDITOR || UNITY_STANDALONE_WIN)
#pragma warning restore 414
#endif
                /// <summary>
                /// The string name of this class. Used in logs.
                /// </summary>
                protected override string Typename { get { return "Request"; } }

                /// <summary>
                /// Initializes the parameter list for the GenvidChecker.
                /// Called before the checker iterates on the parameters.
                /// </summary>
                protected override void Init()
                {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                    m_RequestCallback = new GenvidSDK.RequestCallback(RequestCallbackFunction);
                    m_SubscribedIds = Settings;
#endif
                }

                /// <summary>
                /// Cleans up request-specific members.
                /// Done after the checker finishes unsubscribing the requests.
                /// </summary>
                protected override void Term()
                {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                    m_RequestCallback = null;
                    m_SubscribedIds = null;
#endif
                }

                /// <summary>
                /// Subscribes a request.
                /// </summary>
                /// <param name="topic">Topic of the request.</param>
                /// <param name="userData">The user data associated with the request.</param>
                /// <returns>The operation status. GenvidSDK.success if everything went well.</returns>
                protected override GenvidSDK.Status SubscribeImpl(string topic, IntPtr userData)
                {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                    return GenvidSDK.SubscribeRequest(topic, m_RequestCallback, userData);
#else
                return GenvidSDK.Status.Success; 
#endif
                }

                /// <summary>
                /// Unsubscribes a request.
                /// </summary>
                /// <param name="topic">Topic of the request.</param>
                /// <param name="userData">The user data associated with the request.</param>
                /// <returns>The operation status. GenvidSDK.success if everything went well.</returns>
                protected override GenvidSDK.Status UnsubscribeImpl(string topic, IntPtr userData)
                {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                    return GenvidSDK.UnsubscribeRequest(topic, m_RequestCallback, userData);
#else
                return GenvidSDK.Status.Success;
#endif
                }

                /// <summary>
                /// Used to invoke the request-reception callback when a new request is processed by the checker.
                /// </summary>
                /// <param name="dataEvent">Container with the parameters and received data.</param>
                protected override void OnInvokeFunction(DataFunction<GenvidRequestParameters, GenvidSDK.RequestResult> dataEvent)
                {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                    dataEvent.@event.Result = dataEvent.Data;
                    dataEvent.@event.UserData = dataEvent.UserData;
                    dataEvent.@event.OnRequestReceived();
#endif
                }

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                /// <summary>
                /// Add a request to the list of received requests.
                /// </summary>
                /// <param name="result">The request result.</param>
                /// <param name="userData">The user data associated with the request.</param>
                void RequestCallbackFunction(GenvidSDK.RequestResult result, IntPtr userData)
                {
                    try
                    {
                        PushData(result.topic, result, userData);                        
                    }
                    catch (Exception e)
                    {
                            Debug.LogError("Exception during Request Callback: " + e.ToString());                        
                    }
                }
#endif
            }
        }
    }
}