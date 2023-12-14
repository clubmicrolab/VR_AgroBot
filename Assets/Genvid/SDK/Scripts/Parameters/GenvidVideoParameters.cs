using System;
using UnityEngine;
using Genvid.Plugin.Listener;

namespace Genvid
{
    namespace Plugin
    {
        /// <summary>
        /// Specifies what parameters to set for a video stream.
        /// </summary>
        [CreateAssetMenu(fileName = "Genvid Video Parameters", menuName = "Genvid/Create Video Parameters", order = 1)]
        [Serializable]
        public class GenvidVideoParameters : GenvidParametersBase<IGenvidListenerBase>
        {
            /// <summary>
            /// Useful function to create an instance of GenvidVideoParameters.
            /// ---------------------------------------------------------------
            /// To comply with Unity ScriptableObject instance creation, 
            /// you should not use the new keyword.
            /// Unity ScriptableObject needs to be created using the Unity engine.
            /// If not, Unity will report the warning message:
            /// "ClassName must be instantiated using the ScriptableObject.CreateInstance method instead of new ClassName."
            /// </summary>
            /// <param name="name">ID of the video stream.</param>
            /// <param name="framerate">Framerate at which the video will be sent.</param>
            /// <param name="captureType">Which type of capture: Automatic or Texture.</param>
            /// <param name="videoSource">Video source used when setting captureType to 'Texture'.</param>
            /// <returns></returns>
            public static GenvidVideoParameters Create(string name, float framerate = 30.0f, eCaptureType captureType = eCaptureType.Automatic)
            {
                var video = ScriptableObject.CreateInstance<GenvidVideoParameters>();
                video.Id = name;
                video.Framerate = framerate;
                video.CaptureType = captureType;
                return video;
            }

            /// <summary>
            /// Specifies whether to use the SDK auto-capture or a Unity capture-source.
            /// </summary>
            public enum eCaptureType
            {
                Automatic,
                Texture
            }

            /// <summary>
            /// The stream framerate.
            /// </summary>
            [Range(30.0f, 60.0f)]
            [Tooltip("Video Stream framerate")]
            public float Framerate = 30.0f;

            /// <summary>
            /// The stream capture type: Auto or Unity source.
            /// </summary>
            [Tooltip("Video Capture Type")]
            public eCaptureType CaptureType;
        }
    }
}