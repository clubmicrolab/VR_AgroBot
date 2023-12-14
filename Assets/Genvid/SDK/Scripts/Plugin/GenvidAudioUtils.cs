using UnityEngine;
using System;
using GenvidSDKCSharp;

namespace Genvid
{
    namespace Plugin
    {
        /// <summary>
        /// Utility for facilitating SDK usage within the context of audio.
        /// </summary> 
        public static class GenvidAudioUtils
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
            public static bool Verbose { get; set; }

            /// <summary>
            /// Retrieves the format for a given audio stream.
            /// </summary>
            /// <param name="streamId">ID of the audio stream.</param>
            /// <returns>16S or 32F if successful, GenvidSDK.AudioFormat.Unknown if the parameter can't be retrieved.</returns>
            public static GenvidSDK.AudioFormat GetAudioFormat(string streamId)
            {
                int paramReceived = 0;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                var gvStatus = GenvidSDK.GetParameter(streamId, "audio.format", ref paramReceived);
                if (GenvidSDK.StatusFailed(gvStatus))
                {
                    if(LogError)
                    {
                        Debug.LogError("Error while trying to get the audio format: " + GenvidSDK.StatusToString(gvStatus));
                    }
                    return GenvidSDK.AudioFormat.Unknown;
                }
                else if(Verbose)
                {
                    Debug.Log("Genvid Get audio format performed correctly.");
                }
#endif
                return (GenvidSDK.AudioFormat)paramReceived;
            }

            /// <summary>
            /// Assign the format for a given audio stream: 16S or 32F.
            /// </summary>
            /// <param name="streamId">ID of the audio stream.</param>
            /// <param name="audioFormat">The format of enum type GenvidSDK.AudioFormat.</param>
            /// <returns>True if format set, false otherwise.</returns>
            public static bool SetAudioFormat(string streamId, GenvidSDK.AudioFormat audioFormat)
            {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                var gvStatus = GenvidSDK.SetParameter(streamId, "audio.format", (int)audioFormat);
                if (GenvidSDK.StatusFailed(gvStatus))
                {
                    if(LogError)
                    {
                        Debug.LogError("Error while trying to set the audio format: " + GenvidSDK.StatusToString(gvStatus));
                    }
                    return false;
                }
                else  if(Verbose)
                {
                    Debug.Log("Genvid Set audio format performed correctly.");
                }
#endif
                return true;
            }

            /// <summary>
            /// Retrieve the number of channels of a given audio stream.
            /// </summary>
            /// <param name="streamId">ID of the audio stream.</param>
            /// <returns>Between 1 and 6 (inclusively) channels if retrieval successful, -1 otherwise.</returns>
            public static int GetAudioChannels(string streamId)
            {
                int paramReceived = 0;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                var gvStatus = GenvidSDK.GetParameter(streamId, "audio.channels", ref paramReceived);
                if (GenvidSDK.StatusFailed(gvStatus))
                {
                    if (LogError)
                    {
                        Debug.LogError("Error while trying to get the audio channels: " + GenvidSDK.StatusToString(gvStatus));
                    }
                    return -1;
                }
                else if (Verbose)
                {
                    Debug.Log("Genvid Get audio channels performed correctly.");
                }
#endif
                return paramReceived;
            }

            /// <summary>
            /// Assign the number of channels of a given audio stream.
            /// </summary>
            /// <param name="streamId">ID of the audio stream.</param>
            /// <param name="channels">The number of channels to assign. 1 <= channels <= 6.</param>
            /// <returns>True if assignment successful, false otherwise.</returns>
            public static bool SetAudioChannels(string streamId, int channels)
            {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                var gvStatus = GenvidSDK.SetParameter(streamId, "audio.channels", channels);
                if (GenvidSDK.StatusFailed(gvStatus))
                {
                    if (LogError)
                    {
                        Debug.LogError("Error while trying to set the audio channels: " + GenvidSDK.StatusToString(gvStatus));
                    }
                    return false;
                }
                else if (Verbose)
                {
                    Debug.Log("Genvid Set audio channels performed correctly.");
                }
#endif
                return true;
            }

            /// <summary>
            /// Retrieve the sampling rate of a given audio stream.
            /// </summary>
            /// <param name="streamId">ID of the audio stream.</param>
            /// <returns>Between 11025 and 48000 (inclusively) if successful, -1 otherwise.</returns>
            public static int GetAudioRate(string streamId)
            {
                int paramReceived = 0;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                var gvStatus = GenvidSDK.GetParameter(streamId, "audio.rate", ref paramReceived);
                if (GenvidSDK.StatusFailed(gvStatus))
                {
                    if (LogError)
                    {
                        Debug.LogError("Error while trying to get the audio rate: " + GenvidSDK.StatusToString(gvStatus));
                    }
                    return -1;
                }
                else if (Verbose)
                {
                    Debug.Log("Genvid Get audio rate performed correctly.");
                }
#endif
                return paramReceived;
            }

            /// <summary>
            /// Assign the sampling rate of a given audio stream.
            /// </summary>
            /// <param name="streamId">ID of the audio stream.</param>
            /// <param name="audioRate">The rate to assign. 11025 <= channels <= 48000.</param>
            /// <returns>True if assignment successful, false otherwise.</returns>
            public static bool SetAudioRate(string streamId, int audioRate)
            {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                var gvStatus = GenvidSDK.SetParameter(streamId, "audio.rate", audioRate);
                if (GenvidSDK.StatusFailed(gvStatus))
                {
                    if (LogError)
                    {
                        Debug.LogError("Error while trying to set the audio rate: " + GenvidSDK.StatusToString(gvStatus));
                    }
                    return false;
                }
                else if (Verbose)
                {
                    Debug.Log("Genvid Set audio rate performed correctly.");
                }
#endif
                return true;
            }

            /// <summary>
            /// Retrieve the granularity of a given audio stream.
            /// Granularity represents the frequency at which the SDK can treat incoming data.
            /// Granularity is usually equivalent to the sampling rate.
            /// </summary>
            /// <param name="streamId">ID of the audio stream.</param>
            /// <returns>Integer >= 1 if successful, -1 otherwise.</returns>
            public static int GetAudioGranularity(string streamId)
            {
                int paramReceived = 0;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                var gvStatus = GenvidSDK.GetParameter(streamId, "granularity", ref paramReceived);
                if (GenvidSDK.StatusFailed(gvStatus))
                {
                    if (LogError)
                    {
                        Debug.LogError("Error while trying to get the audio granularity: " + GenvidSDK.StatusToString(gvStatus));
                    }
                    return -1;
                }
                else if (Verbose)
                {
                    Debug.Log("Genvid Get audio granularity performed correctly.");
                }
#endif
                return paramReceived;
            }

            /// <summary>
            /// Assign the granularity of a given audio stream.
            /// Granularity represents the frequency at which the SDK can process incoming data.
            /// Granularity is usually equivalent to the sampling rate.
            /// </summary>
            /// <param name="streamId">ID of the audio stream.</param>
            /// <param name="granularity">The granularity to assign. 1 <= granularity.</param>
            /// <returns>True if assignment successful, false otherwise.</returns>
            public static bool SetAudioGranularity(string streamId, int granularity)
            {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                var gvStatus = GenvidSDK.SetParameter(streamId, "granularity", granularity);
                if (GenvidSDK.StatusFailed(gvStatus))
                {
                    if (LogError)
                    {
                        Debug.LogError("Error while trying to set the audio granularity: " + GenvidSDK.StatusToString(gvStatus));
                    }
                    return false;
                }
                else if (Verbose)
                {
                    Debug.Log("Genvid Set audio granularity performed correctly.");
                }
#endif
                return true;
            }

            /// <summary>
            /// Enable or disable WASAPI usage of a given audio stream.
            /// Enabling WASAPI mode will start automatic audio capture.
            /// Disabling WASAPI mode will stop automatic audio capture if it was initiated.
            /// </summary>
            /// <param name="streamId">ID of the audio stream.</param>
            /// <param name="enabled">True to enable, false to disable.</param>
            /// <returns>True if successful, false otherwise.</returns>
            public static bool SetAudioWasapiMode(string streamId, bool enabled)
            {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                var status = GenvidSDK.SetParameter(streamId, "Audio.Source.WASAPI", enabled ? 1 : 0);
                if (GenvidSDK.StatusFailed(status))
                {
                    Debug.LogError("Error while setting the parameter for the audio: " + GenvidSDK.StatusToString(status));
                    return false;
                }
                else if (Verbose)
                {
                    Debug.Log("Genvid Set Parameter WASAPI performed correctly.");
                }
#endif
                return true;
            }

            /// <summary>
            /// Submit data to a given audio stream.
            /// Use template to specify the sample format (short => 16S and float => 32F).
            /// </summary>
            /// <param name="streamId">ID of the audio stream.</param>
            /// <param name="data">The data to submit.</param>
            /// <param name="channels">Deprecated. To remove.</param>
            /// <returns></returns>
            public static void SubmitAudioData<T>(string streamId, float[] data, int channels)
            {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                long tc = GenvidSDK.GetCurrentTimecode();

                if (typeof(T) == typeof(short[]))
                {
                    var shortData = ConvertAudioF32ToS16(data);
                    var status = GenvidSDK.SubmitAudioData(tc, streamId, shortData);
                    if (GenvidSDK.StatusFailed(status))
                    {
                        Debug.LogError("Error while submitting the audio data to the " + streamId + " stream: " + GenvidSDK.StatusToString(status));
                    }
                    else if (Verbose)
                    {
                        Debug.Log("Genvid Submit audio data performed correctly.");
                    }
                }
                else if (typeof(T) == typeof(float[]))
                {
                    var status = GenvidSDK.SubmitAudioData(tc, streamId, data);
                    if (GenvidSDK.StatusFailed(status))
                    {
                        Debug.LogError("Error while submitting the audio data to the " + streamId + " stream: " + GenvidSDK.StatusToString(status));
                    }
                    else if (Verbose)
                    {
                        Debug.Log("Genvid Submit audio data performed correctly.");
                    }
                }
                else
                {
                    Debug.LogError("Unknown audio format.");
                }
#endif
            }

            /// <summary>
            /// Convert F32 to S16.
            /// </summary>
            /// <param name="streamId">ID of the audio stream.</param>
            /// <param name="data">The data to submit.</param>
            /// <param name="channels">Deprecated. To remove.</param>
            /// <returns></returns>
            public static short[] ConvertAudioF32ToS16(float[] dataSource)
            {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                short[] dataDest = new short[dataSource.Length];

                for (int i = 0; i < dataSource.Length; i++)
                {
                    int value = (int)(dataSource[i] * 32768.0f);
                    // Clamp the value to avoid audio glitching.
                    dataDest[i] = (short)Math.Min(Math.Max(value, -32768), 32767);
                }

                return dataDest;
#else
                return null;
#endif
            }

            /// <summary>
            /// Initialize an audio stream with the configuration of the current audio device.
            /// </summary>
            /// <param name="streamId">ID of the audio stream.</param>
            /// <param name="audioFormat">Specify the audio format: F32 or S16.</param>
            /// <param name="sampleRate">Retrieve the sample rate of the current configuration.</param>
            /// <param name="channels">Retrieve the number of channels of the current configuration.</param>
            /// <returns>True if all the parameters are successfully set, false otherwise.</returns>
            public static bool SetupGenvidUnityAudio(string streamdId, GenvidSDK.AudioFormat audioFormat, out int sampleRate, out int channels)
            {
                bool result = true;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                var config = AudioSettings.GetConfiguration();
                sampleRate = config.sampleRate;
                switch (config.speakerMode)
                {
                    case AudioSpeakerMode.Mono: channels = 1; break;
                    case AudioSpeakerMode.Stereo: channels = 2; break;
                    default:
                        {
                            Debug.LogError("Unsupported audio speaker mode! Please use Mono or Stereo.");
                            channels = 0;
                            break;
                        }
                }

                Debug.Log("Audio sample rate is " + sampleRate);
                Debug.Log("Audio channels is " + channels);
                Debug.Log("DSP buffer size is " + config.dspBufferSize);
                
                result &= GenvidAudioUtils.SetAudioFormat(streamdId, audioFormat);
                result &= GenvidAudioUtils.SetAudioRate(streamdId, sampleRate);
                result &= GenvidAudioUtils.SetAudioChannels(streamdId, channels);
#else
                sampleRate = 0;
                channels = 0;
#endif
                return result;
            }

            /// <summary>
            /// Uses the first-found audio-listener to attach an audio stream filter to.
            /// </summary>
            /// <param name="gameObject">Object to which a newly created listener is attached.</param>
            /// <param name="genvidAudioParametersListener">Audio Listener set by the user using GenvidAudioParameters.</param>
            /// <param name="audioListener">Reference to the found or created listener.</param>
            /// <param name="audioStreamFilter">Reference to the audio filter attached to the listener.</param>
            /// <returns></returns>
            public static void SetupUnityAudioListeners(GameObject gameObject, AudioListener genvidAudioParametersListener, out AudioListener audioListener, out AudioStreamFilter audioStreamFilter)
            {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                if (genvidAudioParametersListener != null)
                {
                    audioListener = genvidAudioParametersListener;
                }
                else
                {
                    audioListener = UnityEngine.Object.FindObjectOfType<AudioListener>();
                }
                Debug.Log("Using " + audioListener.ToString() + " as the AudioListener to capture the Game Audio.");

                if (audioListener == null)
                {
                    Debug.LogWarning("No AudioListener was found. Adding AudioListener...");
                    audioListener = gameObject.AddComponent<AudioListener>();
                }

                audioStreamFilter = audioListener.GetComponent<AudioStreamFilter>();
                if (audioStreamFilter == null)
                {
                    audioStreamFilter = audioListener.gameObject.AddComponent<AudioStreamFilter>();
                }
#else
                audioListener = null;
                audioStreamFilter = null;
#endif
            }
        }
    }
}