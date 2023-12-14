using UnityEngine;
using System;
using GenvidSDKCSharp;
using Genvid;

namespace Genvid
{
    namespace Plugin
    {
        namespace Stream
        {
            /// <summary>
            /// Helper class that handles initializing and terminating the audio stream.
            /// </summary>
            [Serializable]
            public class Audio : IGenvidPlugin
            {
                /// <summary>
                /// Audio parameters specified by the user.
                /// </summary>
                public GenvidAudioParameters Settings;

                /// <summary>
                /// True if the audio stream has been initialized, false otherwise. 
                /// </summary>
                public bool IsInitialized { get; private set; }

                /// <summary>
                /// Specifies logging verbosity. Toggle on for more logging info.
                /// </summary>
                public bool VerboseLog { get; set; }

                /// <summary>
                /// The sampling rate.
                /// </summary>
                public int AudioRate { get; private set; }

                /// <summary>
                /// The number of audio channels.
                /// </summary>
                public int AudioChannels { get; private set; }

                /// <summary>
                /// The current audio listener.
                /// </summary>
                private AudioListener m_AudioListener;

                /// <summary>
                /// The stream filter used with the listener.
                /// </summary>
                private AudioStreamFilter m_AudioStreamFilter;

                /// <summary>
                /// Intializes the audio stream and listeners depending on the selected audio mode.
                /// </summary>
                /// <returns>True if stream was created successfully, false otherwise.</returns>
                public bool Initialize()
                {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                    if (!IsInitialized && Settings != null)
                    {
                        GenvidStreamUtils.VerboseLog = VerboseLog;
                        if (GenvidStreamUtils.CreateStream(Settings.Id))
                        {
                            if (Settings.AudioMode == GenvidAudioParameters.AudioCapture.None)
                            {
                                Debug.LogWarning("No audio mode set. There will be no audio capture!");
                            }
                            else if (Settings.AudioMode == GenvidAudioParameters.AudioCapture.WASAPI)
                            {
                                Debug.Log("Audio mode set to Wasapi.");
                                GenvidAudioUtils.SetAudioWasapiMode(Settings.Id, true);
                            }
                            else if (Settings.AudioMode == GenvidAudioParameters.AudioCapture.Unity)
                            {
                                Debug.Log("Audio mode set to Unity.");

                                GenvidAudioUtils.SetupUnityAudioListeners(GenvidPlugin.Instance.gameObject, Settings.Listener, out m_AudioListener, out m_AudioStreamFilter);

                                int audiorate;
                                int channels;
                                if (GenvidAudioUtils.SetupGenvidUnityAudio(Settings.Id, Settings.AudioFormat, out audiorate, out channels))
                                {
                                    AudioRate = audiorate;
                                    AudioChannels = channels;
                                }

                                m_AudioStreamFilter.OnAudioReceivedDataCallback += OnAudioReceivedDataCallback;
                            }

                            IsInitialized = true;
                        }
                    }

                    return IsInitialized;
#else
                return true;
#endif
                }

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                /// <summary>
                /// Submits audio data to the SDK.
                /// </summary>
                /// <param name="data">The data to submit.</param>
                /// <param name="otherwise">The number of channels.</param>
                private void OnAudioReceivedDataCallback(float[] data, int channels)
                {
                    if (Settings.AudioFormat == GenvidSDK.AudioFormat.S16LE)
                    {
                        GenvidAudioUtils.SubmitAudioData<short[]>(Settings.Id, data, channels);
                    }
                    else if (Settings.AudioFormat == GenvidSDK.AudioFormat.F32LE)
                    {
                        GenvidAudioUtils.SubmitAudioData<float[]>(Settings.Id, data, channels);
                    }
                }
#endif
                /// <summary>
                /// Destroys the audio stream and listeners.
                /// </summary>
                /// <returns>True if stream was successfully destroyed, false otherwise.</returns>
                public bool Terminate()
                {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                    if (IsInitialized)
                    {
                        if (Settings.AudioMode == GenvidAudioParameters.AudioCapture.Unity)
                        {
                            m_AudioStreamFilter.OnAudioReceivedDataCallback -= OnAudioReceivedDataCallback;
                            UnityEngine.Object.Destroy(m_AudioStreamFilter);
                            UnityEngine.Object.Destroy(m_AudioListener);
                        }

                        if (GenvidStreamUtils.DestroyStream(Settings.Id))
                        {
                            IsInitialized = false;
                        }
                    }

                    return !IsInitialized;
#else
                return true;
#endif
                }

                /// <summary>
                /// Empty override of the IGenvidPlugin interface.
                /// All setup is done during the initialization phase.
                /// </summary>
                public void Start()
                {
                    /* Nothing to do*/
                }

                /// <summary>
                /// Empty override of the IGenvidPlugin interface.
                /// No need to update as the sampling is done automatically.
                /// </summary>
                public void Update()
                {
                    /* Nothing to do*/
                }
            }
        }
    }

}