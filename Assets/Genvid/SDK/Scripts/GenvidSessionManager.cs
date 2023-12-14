using UnityEngine;
using System.Collections;
using GenvidSDKCSharp;
using System;

namespace Genvid
{
    namespace Plugin
    {
        /// <summary>
        /// The session manager is responsible for driving
        /// the events of its sessions.
        /// </summary>
        [Serializable]
        public class SessionManager : IGenvidPlugin
        {
            /// <summary>
            /// The session manages all components necessary for a complete
            /// streaming experience: audio, video, data streams, commands, and events.
            /// </summary>
            [SerializeField]
            public Session Session;

            /// <summary>
            /// Keeps track of the session state.
            /// Toggled on when initialization succeeds.
            /// Toggled off when the session terminates.
            /// </summary> 
            public bool IsInitialized { get; private set; }

            /// <summary>
            /// Toggled on when the SDK initialization succeeds.
            /// </summary> 
            public bool IsSDKInitialized { get; private set; }

            /// <summary>
            /// Specifies logging verbosity. Toggle on for more logging info.
            /// </summary>
            public bool VerboseLog { get; set; }

            /// <summary>
            /// Initializes the SDK and the session.
            /// </summary>
            /// <returns>True if all components initialize correctly, false otherwise.</returns>
            public bool Initialize()
            {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                if (!IsInitialized)
                {
                    GenvidSDK.Status gvStatus = GenvidSDK.Initialize();
                    if (GenvidSDK.StatusFailed(gvStatus))
                    {
                        Debug.LogError("Error while initializing GenvidSDK: " + GenvidSDK.StatusToString(gvStatus));
                        return false;
                    }

                    Debug.Log("GenvidSDK.Initialize() performed correctly.");
                    IsSDKInitialized = true;
                    IsInitialized = Session.Initialize();
                }

                return IsInitialized;
#else
                return true;
#endif
            }

            /// <summary>
            /// Terminates the session and SDK.
            /// </summary>
            /// <returns>True if all components terminate correctly, false otherwise.</returns>
            public bool Terminate()
            {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                if (IsSDKInitialized)
                {
                    if (Session != null)
                    {
                        IsInitialized = Session.Terminate();
                    }

                    var gvStatus = GenvidSDK.Terminate();
                    if (GenvidSDK.StatusFailed(gvStatus))
                    {
                        Debug.LogError("Error while running the terminate process: " + GenvidSDK.StatusToString(gvStatus));
                        return false;
                    }

                    Debug.Log("GenvidSDK.Terminate() performed correctly.");
                }
                return IsInitialized;
#else
                return true;
#endif
            }

            /// <summary>
            /// Starts the session.
            /// Called after initialization and before the first update.
            /// </summary>
            public void Start()
            {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                if (IsInitialized)
                {
                    Session.Start();
                }
#endif
            }

            /// <summary>
            /// Evaluates if events have been received and updates the session.
            /// Called once per frame.
            /// </summary>
            public void Update()
            {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                if (IsInitialized)
                {
                    var gvStatus = GenvidSDK.CheckForEvents();
                    if ((GenvidSDK.StatusFailed(gvStatus)) && gvStatus != GenvidSDK.Status.ConnectionTimeout)
                    {
                        Debug.LogError("Error while running CheckForEvents: " + GenvidSDK.StatusToString(gvStatus));
                    }
                    else if (VerboseLog)
                    {
                        Debug.Log("Genvid CheckForEvents performed correctly.");
                    }

                    Session.Update();
                }
#endif
            }
        }
    }
}