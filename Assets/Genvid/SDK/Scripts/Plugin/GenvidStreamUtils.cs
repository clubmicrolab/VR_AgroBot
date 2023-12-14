using GenvidSDKCSharp;
using UnityEngine;
using System;
using System.Collections;
using System.Text;

namespace Genvid
{
    namespace Plugin
    {
        /// <summary>
        /// Helper class facilitating stream configuration.
        /// </summary> 
        public static class GenvidStreamUtils
        {
            /// <summary>
            /// Logs an error message if the function failed.
            /// </summary>
            private static bool m_LogError = true;

            /// <summary>
            /// Get/Set Helper for the member logger.
            /// </summary>
            public static bool LogError { get { return m_LogError; } set { m_LogError = value; } }

            /// <summary>
            /// Specifies logging verbosity. Toggle on for more logging info.
            /// </summary>
            public static bool VerboseLog { get; set; }

            /// <summary>
            /// Creates a stream.
            /// </summary>
            /// <param name="streamId">ID of the stream.</param>
            /// <returns>True if the stream is successfully created, false otherwise.</returns>
            public static bool CreateStream(string streamId)
            {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                var status = GenvidSDK.CreateStream(streamId);
                if (GenvidSDK.StatusFailed(status))
                {
                    if (LogError)
                    {
                        Debug.LogError("Error while creating the " + streamId + " stream: " + GenvidSDK.StatusToString(status));
                    }
                    return false;
                }
                else if(VerboseLog)
                {
                    Debug.Log("Genvid CreateStream named " + streamId + " performed correctly.");
                }
#endif
                return true;
            }

            /// <summary>
            /// Destroys a stream.
            /// </summary>
            /// <param name="streamId">ID of the stream.</param>
            /// <returns>True if the stream is successfully destroyed, false otherwise.</returns>
            public static bool DestroyStream(string streamId)
            {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                var status = GenvidSDK.DestroyStream(streamId);
                if (GenvidSDK.StatusFailed(status))
                {
                    if (LogError)
                    {
                        Debug.LogError("Error while destroying the " + streamId + " stream: " + GenvidSDK.StatusToString(status));
                    }
                    return false;
                }
                else if (VerboseLog)
                {
                    Debug.Log("Genvid DestroyStream named " + streamId + " performed correctly.");
                }
#endif
                return true;
            }

            /// <summary>
            /// Retrieves a stream parameter. Integer value expected.
            /// </summary>
            /// <param name="streamId">ID of the stream.</param>
            /// <param name="paramKey">The parameter key.</param>
            /// <param name="paramValue">The value output. Unchanged in case of an error.</param>
            /// <returns>The request status feedback.</returns>
            public static GenvidSDK.Status GetParameter(object streamID, string paramKey, ref int paramValue)
            {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                var gvStatus = GenvidSDK.GetParameter(streamID, paramKey, ref paramValue);
                if (GenvidSDK.StatusFailed(gvStatus))
                {
                    if (LogError)
                    {
                        Debug.LogError("Error while getting int parameter '" + paramKey + "' for stream '" + streamID + "', status: " + GenvidSDK.StatusToString(gvStatus));
                    }
                }
                else if(VerboseLog)
                {
                    Debug.Log("Genvid GetParameter '" + paramKey + "' performed correctly.");
                }
                return gvStatus;
#else
                return GenvidSDK.Status.Success;
#endif
            }

            /// <summary>
            /// Retrieves a stream parameter. Float value expected.
            /// </summary>
            /// <param name="streamId">ID of the stream.</param>
            /// <param name="paramKey">The parameter key.</param>
            /// <param name="paramValue">The value output. Unchanged in case of an error.</param>
            /// <returns>The request status feedback.</returns>
            public static GenvidSDK.Status GetParameter(object streamID, string paramKey, ref float paramValue)
            {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                var gvStatus = GenvidSDK.GetParameter(streamID, paramKey, ref paramValue);
                if (GenvidSDK.StatusFailed(gvStatus))
                {
                    if (LogError)
                    {
                        Debug.LogError("Error while getting float parameter '" + paramKey + "' for stream '" + streamID + "', status: " + GenvidSDK.StatusToString(gvStatus));
                    }
                }
                else if (VerboseLog)
                {
                    Debug.Log("Genvid GetParameter '" + paramKey + "' performed correctly.");
                }
                return gvStatus;
#else
                return GenvidSDK.Status.Success;
#endif
            }

            /// <summary>
            /// Sets a stream parameter. Integer value expected.
            /// </summary>
            /// <param name="streamId">ID of the stream.</param>
            /// <param name="paramKey">The parameter key.</param>
            /// <param name="paramValue">The value to set.</param>
            /// <returns>The request status feedback.</returns>
            public static GenvidSDK.Status SetParameter(object streamID, string paramKey, int paramValue)
            {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                return GenvidSDK.SetParameter(streamID, paramKey, paramValue);
#else
                return GenvidSDK.Status.Success;
#endif
            }

            /// <summary>
            /// Sets a stream parameter. Float value expected.
            /// </summary>
            /// <param name="streamId">ID of the stream.</param>
            /// <param name="paramKey">The parameter key.</param>
            /// <param name="paramValue">The value to set.</param>
            /// <returns>The request status feedback.</returns>
            public static GenvidSDK.Status SetParameter(object streamID, string paramKey, float paramValue)
            {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                return GenvidSDK.SetParameter(streamID, paramKey, paramValue);
#else
                return GenvidSDK.Status.Success;
#endif
            }

            /// <summary>
            /// Retrieves the framerate of a stream.
            /// </summary>
            /// <param name="streamName">ID of the stream.</param>
            /// <returns>The framerate. NaN if the parameter request is unsuccessful.</returns>
            public static float GetFrameRate(String streamName)
            {
                float floatParamReceived = 0.0f;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                var gvStatus = GetParameter(streamName, "framerate", ref floatParamReceived);
                if (GenvidSDK.StatusFailed(gvStatus))
                {
                    return float.NaN;
                }
#endif
                return floatParamReceived;
            }

            /// <summary>
            /// Sets the framerate of a stream.
            /// </summary>
            /// <param name="streamName">ID of the stream.</param>
            /// <param name="framerate">The new framerate.</param>
            /// <returns>True if the request is successful, false otherwise.</returns>
            public static bool SetFrameRate(String streamName, float framerate)
            {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                return SetParameter(streamName, "framerate", framerate) == GenvidSDK.Status.Success;
#else
                return true;
#endif
            }

            /// <summary>
            /// Retrieves the stream granularity.
            /// Granularity represents the frequency at which the SDK can process incoming data.
            /// Granularity is usually equivalent to the sampling rate or framerate.
            /// </summary>
            /// <param name="streamName">ID of the stream.</param>
            /// <returns>The granularity. NaN if the parameter request is unsuccessful.</returns>
            public static float GetGranularity(String streamName)
            {
                float floatParamReceived = 0.0f;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                var gvStatus = GetParameter(streamName, "granularity", ref floatParamReceived);
                if (GenvidSDK.StatusFailed(gvStatus))
                {
                    return float.NaN;
                }
#endif
                return floatParamReceived;
            }

            /// <summary>
            /// Sets the stream granularity.
            /// Granularity represents the frequency at which the SDK can process incoming data.
            /// Granularity is usually equivalent to the sampling rate or framerate.
            /// </summary>
            /// <param name="streamName">ID of the stream.</param>
            /// <param name="granularity">The new granularity.</param>
            /// <returns>True if the request is successful, false otherwise.</returns>
            public static bool SetGranularity(String streamName, float granularity)
            {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                return SetParameter(streamName, "granularity", granularity) == GenvidSDK.Status.Success;
#else
                return true;
#endif
            }
        }
    }
}