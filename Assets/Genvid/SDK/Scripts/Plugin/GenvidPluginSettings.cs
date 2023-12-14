using GenvidSDKCSharp;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genvid
{
    namespace Plugin
    {
        /// <summary>
        /// Helper class that handles application configuration specified by the user.
        /// </summary>
        [Serializable]
        public class GenvidPluginSettings
        {
            /// <summary>
            /// Specifies whether or not to activate the plugin.
            /// If disabled, the plugin will run without calling any functionality.
            /// </summary>
            public bool ActivateSDK = true;

            /// <summary>
            /// Deprecated.
            /// </summary>
            public bool AutoInitialize = true;

            /// <summary>
            /// Handles the log verbosity level.
            /// </summary>
            public bool ActivateDebugLog = false;

            /// <summary>
            /// Specifies the audio capture mode. WASAPI or Unity.
            /// </summary>
            public GenvidAudioParameters.AudioCapture AudioMode { get; private set; }

            /// <summary>
            /// Keeps track of whether or not the settings were evaluated to avoid doing so more than once.
            /// </summary>
            public bool IsInitialized { get; private set; }

            /// Special property used to disable video data submission throttling.
            /// By default, the video is submitted at the framerate set in GenvidVideo.
            /// By disabling this property, the video submission will follow the game framerate.
            public bool DisableVideoDataSubmissionThrottling { get; private set; }

            /// <summary>
            /// Find user input about the configuration from the command arguments.
            /// </summary>
            public void Load()
            {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                if (!IsInitialized)
                {
                    var args = Environment.GetCommandLineArgs();
                    for (int i = 0; i < args.Length; i++)
                    {
                        switch (args[i])
                        {
                            case "-Genvid": 
                                ActivateSDK = true; 
                                break;

                            case "-GenvidDebugLog": 
                                ActivateDebugLog = true; 
                                break;

                            case "-GenvidAutoInit": 
                                AutoInitialize = true;
                                break;

                            case "-AudioMode": 
                                ParseAudioMode((i + 1) < args.Length ? args[i] : null);
                                break;

                            case "-DisableVideoDataSubmissionThrottling":
                                Debug.Log("Using 'DisableVideoDataSubmissionThrottling' property.");
                                Debug.Log("Genvid video submission will now follow the game framerate.");
                                DisableVideoDataSubmissionThrottling = true;
                                break;
                        }
                    }

                    IsInitialized = true;
                }
#endif
            }

            /// <summary>
            /// Check if the evaluated value is part of an enumerator's values.
            /// </summary>
            /// <param name="enumValue">The value to evaluate.</param>
            /// <returns>True if the value is part of the enum, false otherwise.</returns>
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            private bool IsEnumDefined<T>(string enumValue)
            {
                foreach (var value in Enum.GetValues(typeof(T)))
                {
                    if (String.Equals(value.ToString(), enumValue, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
                return false;
            }

            /// <summary>
            /// Set the audio mode according to an input string.
            /// </summary>
            /// <param name="argValue">The audio mode as string.</param>
            /// <returns></returns>
            private void ParseAudioMode(string argValue)
            {
                bool isDefined = false;

                if (argValue != null)
                {
                    isDefined = IsEnumDefined<GenvidAudioParameters.AudioCapture>(argValue);

                    if (isDefined)
                    {
                        AudioMode = (GenvidAudioParameters.AudioCapture)Enum.Parse(typeof(GenvidAudioParameters.AudioCapture), argValue, true);
                        Debug.Log("Forcing audio mode to '" + argValue + "'.");
                    }
                    else
                    {
                        Debug.LogError("Failed to parse AudioMode: '" + argValue + "' is unknown.");
                    }
                }
                else
                {
                    Debug.LogError("Failed to find a value for the AudioMode parameter.");
                }

                if (!isDefined)
                {
                    Debug.LogError("Use one of the following AudioModes: " + String.Join(", ", Enum.GetNames(typeof(GenvidAudioParameters.AudioCapture))) + ".");
                }
            }
#endif
        }
    }
}