using System;
using UnityEngine;
using GenvidSDKCSharp;
using Genvid.Plugin.Listener;

namespace Genvid
{
    namespace Plugin
    {
        /// <summary>
        /// Specifies what parameters to set for an audio stream.
        /// </summary>
        [CreateAssetMenu(fileName = "Genvid Audio Parameters", menuName = "Genvid/Create Audio Parameters", order = 0)]
        [Serializable]
        public class GenvidAudioParameters : GenvidParametersBase<IGenvidListenerBase>
        {
            /// <summary>
            /// Useful function to create an instance of GenvidAudioParameters.
            /// ---------------------------------------------------------------
            /// To comply with Unity ScriptableObject instance creation, 
            /// you should not use the new keyword.
            /// Unity ScriptableObject needs to be created using the Unity engine.
            /// If not, Unity will report the warning message:
            /// "ClassName must be instantiated using the ScriptableObject.CreateInstance method instead of new ClassName."
            /// </summary>
            /// <param name="name">ID of the audio stream.</param>
            /// <param name="audioMode">Mode used to capture audio: Unity or WASAPI.</param>
            /// <param name="format">Audio data format: 16bits (S16LE) or 32 bits (F32LE32).</param>
            /// <param name="audioListener">Audio listener used to capture the game audio.</param>
            /// <returns></returns>
            public static GenvidAudioParameters Create(string name, AudioCapture audioMode = AudioCapture.WASAPI, GenvidSDK.AudioFormat format = GenvidSDK.AudioFormat.F32LE, AudioListener audioListener = null)
            {
                var audio = ScriptableObject.CreateInstance<GenvidAudioParameters>();
                audio.Id = name;
                audio.AudioMode = audioMode;
                audio.AudioFormat = format;
                audio.Listener = audioListener;
                return audio;
            }

            /// <summary>
            /// Specifies whether to use the WASAPI auto-capture or a Unity capture-source.
            /// </summary>
            public enum AudioCapture
            {
                None,
                WASAPI,
                Unity
            }

            /// <summary>
            /// The stream capture type: WASAPI or Unity.
            /// </summary>
            public AudioCapture AudioMode;

            /// <summary>
            /// The audio format: F32 or S16.
            /// </summary>
            public GenvidSDK.AudioFormat AudioFormat;

            /// <summary>
            /// Audio listener used to capture the game audio.
            /// </summary>
            public AudioListener Listener;
        }
    }
}