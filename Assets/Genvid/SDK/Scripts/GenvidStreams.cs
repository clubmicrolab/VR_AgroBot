using GenvidSDKCSharp;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Text;
using System.Collections.Generic;
using Genvid;

namespace Genvid
{
    namespace Plugin
    {
        namespace Stream
        {
            /// <summary>
            /// Helper class that handles state changes and data submissions for data streams and annotations.
            /// </summary>
            [Serializable]
            public class Data : IGenvidPlugin
            {
                /// <summary>
                /// Class representing a stream event as a Unity event.
                /// </summary>
                [Serializable]
                public class StreamEvent : UnityEvent<string>
                {
                }

                /// <summary>
                /// List of data-stream parameters.
                /// Streams are created from this list.
                /// </summary>
                public List<GenvidStreamParameters> Settings;

                /// <summary>
                /// Toggled on when stream creation is done.
                /// </summary> 
                public bool IsInitialized { get; private set; }

                /// <summary>
                /// Specifies logging verbosity. Toggle on for more logging info.
                /// </summary>
                public bool VerboseLog { get; set; }

                /// <summary>
                /// Creates all data streams from the stream parameters.
                /// </summary>
                /// <returns>True if all streams are created correctly, false otherwise.</returns>
                public bool Initialize()
                {
                    bool result = true;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                    if (!IsInitialized && Settings != null && Settings.Count > 0)
                    {
                        foreach (var stream in Settings)
                        {
                            if (GenvidStreamUtils.CreateStream(stream.Id))
                            {
                                GenvidStreamUtils.SetFrameRate(stream.Id, stream.Framerate);
                            }
                            else
                            {
                                result = false;
                            }
                        }

                        IsInitialized = true;
                    }
#endif
                    return result;
                }

                /// <summary>
                /// Destroys all data streams.
                /// </summary>
                /// <returns>True if all streams are destroyed correctly, false otherwise.</returns>
                public bool Terminate()
                {
                    bool result = true;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                    if (IsInitialized && Settings != null && Settings.Count > 0)
                    {
                        foreach (var stream in Settings)
                        {
                            result &= GenvidStreamUtils.DestroyStream(stream.Id);
                        }

                        IsInitialized = false;
                    }
#endif
                    return result;
                }

                /// <summary>
                /// Calls the OnStart event assigned to each stream.
                /// </summary>
                public void Start()
                {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                    if (IsInitialized && Settings.Count > 0)
                    {
                        foreach (var stream in Settings)
                        {
                            if (stream == null)
                            {
                                Debug.LogWarning("Genvid Stream is null! Add a GenvidStreamParameters or decrease the size of the list.");
                            }
                            else
                            {
                                if (stream != null)
                                {
                                    stream.OnStart();
                                }
                            }
                        }
                    }
#endif
                }

                /// <summary>
                /// Calls the submit event of each stream at the same framerate as that stream. 
                /// </summary>
                public void Update()
                {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                    if (IsInitialized && Settings.Count > 0)
                    {
                        foreach (var stream in Settings)
                        {
                            if (stream != null)
                            {
                                if (stream.Deadline.IsPassed())
                                {
                                    try
                                    {
                                        stream.OnSubmitStream();
                                    }
                                    catch (Exception e)
                                    {
                                        Debug.LogError("Exception during OnSubmitStream: " + e.ToString());
                                    }
                                    stream.Deadline.Next();
                                }
                            }
                        }
                    }
#endif
                }

                /// <summary>
                /// Submits game data on a given stream.
                /// </summary>
                /// <param name="streamID">ID of the stream.</param>
                /// <param name="data">Data to submit.</param>
                /// <param name="size">Size of data to submit.</param>
                /// <returns>True if the game data is successfully submitted, false otherwise.</returns>
                public bool SubmitGameData(object streamID, byte[] data, int size)
                {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                    if (!IsInitialized)
                    {
                        Debug.LogError(String.Format("Unable to submit game data on nonexistent stream '{0}'.", streamID));
                        return false;
                    }

                    var status = GenvidSDK.SubmitGameData(GenvidSDK.GetCurrentTimecode(), streamID.ToString(), data, size);
                    if (GenvidSDK.StatusFailed(status))
                    {
                        Debug.LogError(String.Format("`SubmitGameData` failed with error: {0}.", GenvidSDK.StatusToString(status)));
                        return false;
                    }

                    if (VerboseLog)
                    {
                        Debug.Log(String.Format("Genvid correctly submitted game data: {0}", data));
                    }
#endif

                    return true;
                }

                /// <summary>
                /// Submits an annotation on a given data stream.
                /// </summary>
                /// <param name="streamID">ID of the stream.</param>
                /// <param name="data">Data to submit.</param>
                /// <param name="size">Size of data to submit.</param>
                /// <returns>True if the annotation is successfully submitted, false otherwise.</returns>
                public bool SubmitAnnotation(object streamID, byte[] data, int size)
                {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                    if (!IsInitialized)
                    {
                        Debug.LogError(String.Format("Unable to submit annotation on nonexistent stream '{0}'.", streamID));
                        return false;
                    }

                    var status = GenvidSDK.SubmitAnnotation(GenvidSDK.GetCurrentTimecode(), streamID.ToString(), data, size);
                    if (GenvidSDK.StatusFailed(status))
                    {
                        Debug.LogError(String.Format("`SubmitAnnotation` failed with error: {0}", GenvidSDK.StatusToString(status)));
                        return false;
                    }

                    if (VerboseLog)
                    {
                        Debug.Log(String.Format("Genvid correctly submitted annotation: {0}", data));
                    }
#endif
                    return true;
                }

                /// <summary>
                /// Submits game data on a given data stream.
                /// </summary>
                /// <param name="streamID">ID of the stream.</param>
                /// <param name="data">Data to submit.</param>
                /// <returns>True if the data is successfully submitted, false otherwise.</returns>
                public bool SubmitGameData(object streamID, byte[] data)
                {
                    return SubmitGameData(streamID, data, data.Length);
                }

                /// <summary>
                /// Submits an annotation on a given data stream.
                /// </summary>
                /// <param name="streamID">ID of the stream.</param>
                /// <param name="data">Data to submit.</param>
                /// <returns>True if the annotation is successfully submitted, false otherwise.</returns>
                public bool SubmitAnnotation(object streamID, byte[] data)
                {
                    return SubmitAnnotation(streamID, data, data.Length);
                }

                /// <summary>
                /// Submits an game data on a given data stream.
                /// </summary>
                /// <param name="streamID">ID of the stream.</param>
                /// <param name="data">Data to submit.</param>
                /// <returns>True if the game data is successfully submitted, false otherwise.</returns>
                public bool SubmitGameData(object streamID, string data)
                {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                    if (data == null)
                    {
                        Debug.LogError("Unable to handle `null` data. Submitting game data failed.");
                        return false;
                    }

                    var dataAsBytes = Encoding.Default.GetBytes(data);
                    return SubmitGameData(streamID, dataAsBytes);
#else
                return true;
#endif
                }

                /// <summary>
                /// Submits an annotation on a given data stream.
                /// </summary>
                /// <param name="streamID">ID of the stream.</param>
                /// <param name="data">Data to submit.</param>
                /// <returns>True if the annotation is successfully submitted, false otherwise.</returns>
                public bool SubmitAnnotation(object streamID, string data)
                {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                    if (data == null)
                    {
                        Debug.LogError("Unable to handle `null` data. Submitting annotation failed.");
                        return false;
                    }

                    var dataAsBytes = Encoding.Default.GetBytes(data);
                    return SubmitAnnotation(streamID, dataAsBytes);
#else
                return true;
#endif
                }

                /// <summary>
                /// Submits game data on a given data stream.
                /// The game-data object is first serialized to JSON.
                /// </summary>
                /// <param name="streamID">ID of the stream.</param>
                /// <param name="data">Data object to submit.</param>
                /// <returns>True if the game data is successfully submitted, false otherwise.</returns>
                public bool SubmitGameDataJSON(object streamID, object data)
                {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                    if (data == null)
                    {
                        Debug.LogError("Unable to handle `null` data. Submitting game data failed.");
                        return false;
                    }

                    var jsonData = GenvidPlugin.SerializeToJSON(data);
                    if (jsonData == null)
                    {
                        Debug.LogError(String.Format("Failed to send game data on stream '{0}' due to a JSON serialization error.", streamID));
                        return false;
                    }

                    return SubmitGameData(streamID, jsonData);
#else
                return true;
#endif
                }

                /// <summary>
                /// Submits an annotation on a given data stream.
                /// The annotation object is first serialized to JSON.
                /// </summary>
                /// <param name="streamID">ID of the stream.</param>
                /// <param name="data">Data object to submit.</param>
                /// <returns>True if the annotation is successfully submitted, false otherwise.</returns>
                public bool SubmitAnnotationJSON(object streamID, object data)
                {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                    if (data == null)
                    {
                        Debug.LogError("Unable to handle `null` data. Submitting annotation failed.");
                        return false;
                    }

                    var jsonData = GenvidPlugin.SerializeToJSON(data);
                    if (jsonData == null)
                    {
                        Debug.LogError(String.Format("Failed to send annotation on stream '{0}' due to a JSON serialization error.", streamID));
                        return false;
                    }

                    return SubmitAnnotation(streamID, jsonData);
#else
                return true;
#endif
                }
            }
        }
    }
}