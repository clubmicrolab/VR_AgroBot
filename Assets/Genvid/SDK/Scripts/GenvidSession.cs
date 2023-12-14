using GenvidSDKCSharp;
using UnityEngine;
using System;
using System.Text;
using Genvid;

namespace Genvid
{
    namespace Plugin
    {
        /// <summary>
        /// The session manages all components necessary for a complete
        /// streaming experience: audio, video, data streams, commands, and events.
        /// </summary>
        [Serializable]
        public class Session : IGenvidPlugin
        {
            /// <summary>
            /// The video component.
            /// </summary>
            [SerializeField]
            private Stream.Video m_Video;

            /// <summary>
            /// The audio component.
            /// </summary>
            [SerializeField]
            private Stream.Audio m_Audio;

            /// <summary>
            /// The data-streams component.
            /// </summary>
            [SerializeField]
            private Stream.Data m_Streams;

            /// <summary>
            /// The events component.
            /// </summary>
            [SerializeField]
            private Channel.Events m_Events;

            /// <summary>
            /// The commands component.
            /// </summary>
            [SerializeField]
            private Channel.Commands m_Commands;

            /// <summary>
            /// The requests component.
            /// </summary>
            [SerializeField]
            private Channel.Requests m_Requests;

            /// <summary>
            /// Keeps track of the component state.
            /// Toggled on when initalization succeeds.
            /// Toggled off when the plugin is disabled.
            /// </summary> 
            public bool IsInitialized { get; private set; }

            /// <summary>
            /// Specifies logging verbosity. Toggle on for more logging info.
            /// </summary>
            public bool VerboseLog { get; set; }

            /// <summary>
            /// The video component getter/setter.
            /// </summary>
            public Stream.Video Video { get { return m_Video; } set { m_Video = value; } }

            /// <summary>
            /// The audio component getter/setter.
            /// </summary>
            public Stream.Audio Audio { get { return m_Audio; } set { m_Audio = value; } }

            /// <summary>
            /// The data streams component getter/setter.
            /// </summary>
            public Stream.Data Data { get { return m_Streams; } set { m_Streams = value; } }

            /// <summary>
            /// The events component getter/setter.
            /// </summary>
            public Channel.Events Events { get { return m_Events; } set { m_Events = value; } }

            /// <summary>
            /// The commands component getter/setter.
            /// </summary>
            public Channel.Commands Commands { get { return m_Commands; } set { m_Commands = value; } }

            /// <summary>
            /// The requests component getter/setter.
            /// </summary>
            public Channel.Requests Requests { get { return m_Requests; } set { m_Requests = value; } }

            /// <summary>
            /// Initializes all components.
            /// </summary>
            /// <returns>True if all components initialize correctly, false otherwise.</returns>
            public bool Initialize()
            {
                bool result = true;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                if (IsInitialized)
                {
                    return true;
                }

                if (Video.Initialize() == false)
                {
                    result = false;
                    Debug.LogError("GenvidSession failed to initialize the video stream!");
                }

                if (Audio.Initialize() == false)
                {
                    result = false;
                    Debug.LogError("GenvidSession failed to initialize an audio stream!");
                }

                if (Data.Initialize() == false)
                {
                    result = false;
                    Debug.LogError("GenvidSession failed to initialize gamedata streams!");
                }

                if (Events.Initialize() == false)
                {
                    result = false;
                    Debug.LogError("GenvidSession failed to initialize Genvid events!");
                }

                if (Commands.Initialize() == false)
                {
                    result = false;
                    Debug.LogError("GenvidSession failed to initialize Genvid commands!");
                }

                if (Requests.Initialize() == false)
                {
                    result = false;
                    Debug.LogError("GenvidSession failed to initialize Genvid requests!");
                }

#endif
                IsInitialized = result;
                return result;
            }

            /// <summary>
            /// Cleans up all components.
            /// </summary>
            /// <returns>True if all components terminate correctly, false otherwise.</returns>
            public bool Terminate()
            {
                bool result = true;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                if (IsInitialized == false)
                {
                    return true;
                }

                if (Requests != null)
                {
                    if (Requests.Terminate() == false)
                    {
                        result = false;
                        Debug.LogError("GenvidSession failed to terminate Genvid requests!");
                    }
                }

                if (Commands != null)
                {
                    if (Commands.Terminate() == false)
                    {
                        result = false;
                        Debug.LogError("GenvidSession failed to terminate Genvid commands!");
                    }
                }

                if (Events != null)
                {
                    if (Events.Terminate() == false)
                    {
                        result = false;
                        Debug.LogError("GenvidSession failed to terminate Genvid events!");
                    }
                }

                if (Data != null)
                {
                    if (Data.Terminate() == false)
                    {
                        result = false;
                        Debug.LogError("GenvidSession failed to terminate gamedata streams!");
                    }
                }

                if (Audio != null)
                {
                    if (Audio.Terminate() == false)
                    {
                        result = false;
                        Debug.LogError("GenvidSession failed to terminate an audio stream!");
                    }
                }

                if (Video != null)
                {
                    if (Video.Terminate() == false)
                    {
                        result = false;
                        Debug.LogError("GenvidSession failed to terminate the video stream!");
                    }
                }
#endif
                IsInitialized = !result;
                return result;
            }

            /// <summary>
            /// Starts all components.
            /// </summary>
            public void Start()
            {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                if (Video.IsInitialized)
                {
                    Video.Start();
                }
                if (Audio.IsInitialized)
                {
                    Audio.Start();
                }
                if (Data.IsInitialized)
                {
                    Data.Start();
                }
                if (Events.IsInitialized)
                {
                    Events.Start();
                }
                if (Commands.IsInitialized)
                {
                    Commands.Start();
                }
                if (Requests.IsInitialized)
                {
                    Requests.Start();
                }
#endif
            }

            /// <summary>
            /// Updates all components. Called once per frame.
            /// </summary>
            public void Update()
            {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                if (Video.IsInitialized)
                {
                    Video.Update();
                }
                if (Audio.IsInitialized)
                {
                    Audio.Update();
                }
                if (Data.IsInitialized)
                {
                    Data.Update();
                }
                if (Events.IsInitialized)
                {
                    Events.Update();
                }
                if (Commands.IsInitialized)
                {
                    Commands.Update();
                }
                if (Requests.IsInitialized)
                {
                    Requests.Update();
                }
#endif
            }

            /// <summary>
            /// Submits a notification.
            /// </summary>
            /// <param name="notificationID">ID of the notification.</param>
            /// <param name="data">Data to submit.</param>
            /// <param name="size">Size of data to submit</param>
            /// <returns>True if the notification was successfully submitted, false otherwise.</returns>
            public bool SubmitNotification(object notificationID, byte[] data, int size)
            {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                if (!IsInitialized)
                {
                    Debug.LogError("Genvid Session is not initialized: Unable to submit notification.");
                    return false;
                }

                var status = GenvidSDK.SubmitNotification(notificationID.ToString(), data, size);

                if (GenvidSDK.StatusFailed(status))
                {
                    Debug.LogError(String.Format("`SubmitNotification` failed with error: {0}.", GenvidSDK.StatusToString(status)));
                    return false;
                }

                if (VerboseLog)
                {
                    Debug.Log(String.Format("Genvid correctly submitted notification: {0}.", data));
                }
#endif
                return true;
            }

            /// <summary>
            /// Submits a notification.
            /// </summary>
            /// <param name="notificationID">ID of the notification.</param>
            /// <param name="data">Data to submit.</param>
            /// <returns>True if the notification was successfully submitted, false otherwise.</returns>
            public bool SubmitNotification(object notificationID, byte[] data)
            {
                return SubmitNotification(notificationID, data, data.Length);
            }

            /// <summary>
            /// Submits a notification.
            /// </summary>
            /// <param name="notificationID">ID of the notification.</param>
            /// <param name="data">Data to submit.</param>
            /// <returns>True if the notification was successfully submitted, false otherwise.</returns>
            public bool SubmitNotification(object notificationID, string data)
            {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                if (data == null)
                {
                    Debug.LogError("Unable to handle `null` data. Submitting notification failed.");
                    return false;
                }

                var dataAsBytes = Encoding.Default.GetBytes(data);
                return SubmitNotification(notificationID, dataAsBytes);
#else
                return true;
#endif
            }

            /// <summary>
            /// Submits a notification.
            /// Notification data object is serialized to JSON before submission.
            /// </summary>
            /// <param name="notificationID">ID of the notification.</param>
            /// <param name="data">Data object to submit</param>
            /// <returns>True if the notification was successfully submitted, false otherwise.</returns>
            public bool SubmitNotificationJSON(object notificationID, object data)
            {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                if (data == null)
                {
                    Debug.LogError("Unable to handle `null` data. Submitting notification failed.");
                    return false;
                }

                var jsonData = GenvidPlugin.SerializeToJSON(data);

                if (jsonData == null)
                {
                    Debug.LogError(String.Format("Failed to send notification with ID '{0}' due to a JSON serialization error.", notificationID));
                    return false;
                }

                return SubmitNotification(notificationID, jsonData);
#else
                return true;
#endif
            }

            /// <summary>
            /// Submits a reply to a request.
            /// </summary>
            /// <param name="topic">Topic of the request.</param>
            /// <param name="replyTo">ReplyTo address.</param>
            /// <param name="reply">Reply to submit.</param>
            /// <param name="size">Size of reply to submit</param>
            /// <returns>True if the reply was successfully submitted, false otherwise.</returns>
            public bool SubmitRequestReply(string topic, string replyTo, byte[] reply, int size)
            {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                if (!IsInitialized)
                {
                    Debug.LogError("Genvid Session is not initialized: Unable to submit reply.");
                    return false;
                }

                var status = GenvidSDK.SubmitRequestReply(topic, replyTo, reply, size);

                if (GenvidSDK.StatusFailed(status))
                {
                    Debug.LogError(String.Format("`SubmitRequestReply` failed with error: {0}.", GenvidSDK.StatusToString(status)));
                    return false;
                }

                if (VerboseLog)
                {
                    Debug.Log(String.Format("Genvid correctly submitted notification: {0}.", reply));
                }
#endif
                return true;
            }

            /// <summary>
            /// Submits a reply to a request.
            /// </summary>
            /// <param name="topic">Topic of the request.</param>
            /// <param name="replyTo">ReplyTo address.</param>
            /// <param name="reply">Reply to submit.</param>
            /// <returns>True if the reply was successfully submitted, false otherwise.</returns>
            public bool SubmitRequestReply(string topic, string replyTo, byte[] reply)
            {
                int size = reply == null ? 0 : reply.Length;
                return SubmitRequestReply(topic, replyTo, reply, size);
            }

            /// <summary>
            /// Submits a reply to a request.
            /// </summary>
            /// <param name="topic">Topic of the request.</param>
            /// <param name="replyTo">ReplyTo address.</param>
            /// <param name="reply">Reply to submit.</param>
            /// <returns>True if the reply was successfully submitted, false otherwise.</returns>
            public bool SubmitRequestReply(string topic, string replyTo, string reply)
            {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                if (reply == null)
                {
                    return SubmitRequestReply(topic, replyTo, null, 0);
                }
                else
                {
                    var dataAsBytes = Encoding.Default.GetBytes(reply);
                    return SubmitRequestReply(reply, replyTo, dataAsBytes);
                }
#else
                return true;
#endif
            }
        }
    }
}