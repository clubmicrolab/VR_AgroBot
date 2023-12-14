using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using GenvidSDKCSharp;

namespace Genvid
{
    namespace Plugin
    {
        namespace Listener
        {
            using GenvidRequestAction = UnityAction<string, GenvidSDK.RequestResult, IntPtr>;

            /// <summary>
            /// A request listener stores a callback definition for what should be done when
            /// a request is received. The request parameters are used as identifiers.
            /// </summary>
            [Serializable]
            public class RequestListener
            {
                /// <summary>
                /// Constructor for the request listener.
                /// </summary>
                /// <param name="request">The request parameters.</param>
                /// <param name="OnRequest">Unity event to call when the request is received.</param>
                public RequestListener(GenvidRequestParameters request, GenvidRequestAction OnRequest)
                {
                    Request = request;
                    if (OnRequest != null)
                    {
                        OnRequestTriggered = new RequestEvent();
                        OnRequestTriggered.AddListener(OnRequest);
                    }
                }

                /// <summary>
                /// The request parameters.
                /// </summary>
                public GenvidRequestParameters Request;

                /// <summary>
                /// Unity event to call when the request is received.
                /// </summary>
                public RequestEvent OnRequestTriggered;
            }

            /// <summary>
            /// Specializes the listener base for requests.
            /// </summary>
            [Serializable]
            public class RequestsListener : GenvidListenerBase<GenvidRequestParameters, RequestListener>, IGenvidRequestListener
            {
                /// <summary>
                /// Registers a request listener with the request-parameter list of listeners.
                /// </summary>
                /// <param name="listener">The request listener.</param>
                /// <returns>The request parameters.</returns>
                protected override GenvidRequestParameters RegisterListener(RequestListener listener)
                {
                    if(listener != null)
                    {
                        listener.Request.RegisterListener(this);
                        return listener.Request;
                    }

                    return null;
                }

                /// <summary>
                /// Removes the request listener from the request parameters.
                /// </summary>
                /// <param name="listener">The request listener.</param>
                /// <returns>The request parameters.</returns>
                protected override GenvidRequestParameters UnregisterListener(RequestListener listener)
                {
                    if (listener != null)
                    {
                        listener.Request.UnregisterListener(this);
                        return listener.Request;
                    }

                    return null;
                }

                /// <summary>
                /// Invokes the request callback defined in the listener.
                /// Used on receiving a request.
                /// </summary>
                /// <param name="requestParams">The request parameters.</param>
                public void OnRequestReceived(GenvidRequestParameters requestParams)
                {
                    RequestListener listener;
                    if (TryGetValue(requestParams, out listener))
                    {
                        if (listener.OnRequestTriggered != null)
                        {
                            listener.OnRequestTriggered.Invoke(requestParams.Result.topic, requestParams.Result, requestParams.UserData);
                        }
                    }
                }
            }

            /// <summary>
            /// MonoBehavior wrapper around the IGenvidRequestListener interface.
            /// </summary>
            public class GenvidRequestsListener : MonoBehaviour, IGenvidRequestListener
            {
                /// <summary>
                /// Request-listener object containing the list of command listeners.
                /// </summary>
                [SerializeField]
                RequestsListener m_Requests;

                /// <summary>
                /// Returns the request-listener container.
                /// </summary>
                public RequestsListener Requests { get { return m_Requests; } }

                /// <summary>
                /// Creates the requests container on MonoBehaviour instantiation.
                /// </summary>
                void Awake()
                {
                    if (m_Requests == null)
                    {
                        m_Requests = new RequestsListener();
                    }
                }

                /// <summary>
                /// Initializes the requests container when the MonoBehaviour becomes active.
                /// </summary>
                public void OnEnable()
                {
                    m_Requests.Initialize();
                    m_Requests.EnableListeners();
                }

                /// <summary>
                /// Cleans up the requests container when the MonoBehaviour becomes inactive.
                /// </summary>
                public void OnDisable()
                {
                    m_Requests.DisableListeners();
                    m_Requests.Terminate();
                }

                /// <summary>
                /// Invokes the request callback defined in the listener.
                /// Used on receiving a request.
                /// </summary>
                /// <param name="requestParams">The request parameters.</param>
                public void OnRequestReceived(GenvidRequestParameters requestParams)
                {
                    m_Requests.OnRequestReceived(requestParams);
                }
            }
        }
    }
}