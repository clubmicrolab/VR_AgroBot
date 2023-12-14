using UnityEngine;
using GenvidSDKCSharp;

namespace Genvid
{
    namespace Plugin
    {
        /// <summary>
        /// Utility for facilitating using the SDK within the context of video.
        /// </summary> 
        public static class GenvidVideoUtils
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
            /// Creates a new video stream.
            /// </summary>
            /// <param name="streamName">ID of the video stream.</param>
            /// <param name="framerate">The stream framerate.</param>
            /// <returns>True if stream creation is successful, false otherwise.</returns>
            public static bool CreateStream(string streamName, float framerate)
            {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                if(!GenvidStreamUtils.CreateStream(streamName))
                {
                    return false;
                }

                return GenvidStreamUtils.SetFrameRate(streamName, framerate);
#else
                return true;
#endif
            }

            /// <summary>
            /// Destroys a video stream.
            /// </summary>
            /// <param name="streamName">ID of the video stream.</param>
            /// <returns>True if stream destruction is successful, false otherwise.</returns>
            public static bool DestroyStream(string streamName)
            {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                if (!GenvidStreamUtils.DestroyStream(streamName))
                {
                    return false;
                }
#endif
                return true;
            }
        }
    }
}