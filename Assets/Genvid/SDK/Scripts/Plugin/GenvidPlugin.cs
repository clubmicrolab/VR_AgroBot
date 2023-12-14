using GenvidSDKCSharp;
using System;
using System.Collections;
using UnityEngine;

namespace Genvid
{
    namespace Plugin
    {
        /// <summary>
        /// The GenvidPlugin interface.
        /// Components implementing this interface are handle the GenvidPlugin through the session manager.
        /// </summary>
        public interface IGenvidPlugin
        {
            /// <summary>
            /// Keeps track of the component state.
            /// Toggled on when initalization succeeds.
            /// Toggled off when the plugin is disabled.
            /// </summary> 
            bool IsInitialized { get; }

            /// <summary>
            /// Specifies logging verbosity. Toggle on for more logging info.
            /// </summary> 
            bool VerboseLog { get; }

            /// <summary>
            /// Called when the GenvidPlugin is enabled or activated.
            /// Don't forget to set IsInitialized to true if everything went well.
            /// </summary> 
            /// <returns>True if successful, false otherwise.</returns>
            bool Initialize();

            /// <summary>
            /// Called when the GenvidPlugin is disabled.
            /// Don't forget to set IsInitialized to false if everything went well.
            /// </summary> 
            /// <returns>True if successful, false otherwise.</returns>
            bool Terminate();

            /// <summary>
            /// Called when Start is called on the GenvidPlugin MonoBehaviour.
            /// Use for post-initialization behavior.
            /// </summary> 
            /// <returns></returns>
            void Start();

            /// <summary>
            /// Called when Update is called on the GenvidPlugin MonoBehaviour.
            /// Use for repeated behavior.
            /// </summary> 
            /// <returns></returns>
            void Update();
        }

        /// <summary>
        /// The GenvidPlugin MonoBehaviour.
        /// The main driver responsible for calling the session manager events.
        /// Also responsible for handling the SDK.
        /// </summary>
        [Serializable]
        public class GenvidPlugin : MonoBehaviour
        {
            /// <summary>
            /// The Genvid settings used to configure the plugin.
            /// </summary>
            [SerializeField]
            public GenvidPluginSettings Settings;

            /// <summary>
            /// The session manager.
            /// Used to control all IGenvidPlugin derived components.
            /// </summary>
            [SerializeField]
            public SessionManager SessionManager;

            /// <summary>
            /// Singleton instance of the GenvidPlugin.
            /// </summary> 
            private static GenvidPlugin m_Instance;

            /// <summary>
            /// Keeps track of the GenvidPlugin state.
            /// The plugin instance will be inert if not initialized.
            /// </summary> 
            public bool IsInitialized { get; private set; }

            /// <summary>
            /// Deprecated.
            /// </summary> 
            private bool IsSDKInitialized { get; set; }

            /// <summary>
            /// Singleton pattern for a single GenvidPlugin instance.
            /// Use this instance only.
            /// </summary> 
            public static GenvidPlugin Instance
            {
                get
                {
                    if (m_Instance == null)
                    {
                        var instances = FindObjectsOfType<GenvidPlugin>();
                        m_Instance = FindObjectOfType<GenvidPlugin>();

                        if (instances.Length > 1)
                        {
                            Debug.LogError("[Singleton] Something went really wrong " +
                                            " - there should never be more than 1 singleton!" +
                                            " Reopening the scene might fix it.");
                            return m_Instance;
                        }

                        if (m_Instance == null)
                        {
                            GameObject singleton = new GameObject();
                            m_Instance = singleton.AddComponent<GenvidPlugin>();
                            singleton.name = "Genvid (Singleton)";

                            DontDestroyOnLoad(singleton);

                            Debug.Log("[Singleton] An instance of " + typeof(SessionManager) +
                                        " is needed in the scene, so '" + singleton +
                                        "' was created with DontDestroyOnLoad.");
                        }
                        else
                        {
                            DontDestroyOnLoad(m_Instance.gameObject);
                            Debug.Log("[Singleton] Using instance already created: " + m_Instance.gameObject.name);
                        }
                    }

                    return m_Instance;
                }
            }

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            /// <summary>
            /// Loads the SDK and plugin settings when the script instance is loaded.
            /// </summary> 
            private void Awake()
            {
                if (GenvidPluginLoader.IsLoaded)
                {
                    return;
                }

                /// Need to keep the instance alive before switching scenes. Mandatory when the Genvid MILES SDK isn't active
                var instanceInit = Instance;

                if (instanceInit == null)
                {
                    Debug.Log("Genvid Singleton instance is null!");
                }
                else
                {
                    /// Load the Genvid Plugin.
                    if (!GenvidPluginLoader.LoadSDK())
                    {
                        Debug.LogError("Failed to load Genvid.dll");
                        return;
                    }

                    /// Load the Genvid Plugin settings.
                    if (Settings == null)
                    {
                        Settings = new GenvidPluginSettings();
                    }

                    Settings.Load();
                }
            }

            /// <summary>
            /// Starts the session manager before the first update call.
            /// </summary> 
            private void Start()
            {
                if (IsInitialized)
                {
                    SessionManager.Start();
                }
            }

            /// <summary>
            /// Keeps the session manager ticking every frame.
            /// </summary> 
            private void Update()
            {
                if (IsInitialized)
                {
                    SessionManager.Update();
                }
            }

            /// <summary>
            /// Initailizes the session manager when the Genvid Plugin is enabled.
            /// </summary> 
            private void OnEnable()
            {
                if (Settings.ActivateSDK)
                {
                    // Initialize Genvid Plugin.
                    Initialize();
                }
            }
            
            /// <summary>
            /// Disables the session manager when the Genvid Plugin is disabled.
            /// </summary>
            private void OnDisable()
            {
                // Terminate the Genvid Plugin.
                Terminate();
            }
#endif
            /// <summary>
            /// Loads the SDK if not already done and initializes the session manager.
            /// </summary>
            /// <returns>True if session manager initialized correctly, false otherwise or if the SDK could not be loaded.</returns>
            public bool Initialize()
            {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                /// Load the Genvid Plugin (ignored if already loaded).
                if (!GenvidPluginLoader.LoadSDK())
                {
                    Debug.LogError("Failed to load Genvid.dll");
                    return false;
                }

                if (!IsInitialized)
                {
                    if (SessionManager != null)
                    {
                        /// Propagate the debug log parameters.
                        SessionManager.VerboseLog = Settings.ActivateDebugLog;
                        SessionManager.Session.VerboseLog = Settings.ActivateDebugLog;
                        SessionManager.Session.Video.VerboseLog = Settings.ActivateDebugLog;
                        SessionManager.Session.Audio.VerboseLog = Settings.ActivateDebugLog;
                        SessionManager.Session.Data.VerboseLog = Settings.ActivateDebugLog;
                        SessionManager.Session.Events.VerboseLog = Settings.ActivateDebugLog;
                        SessionManager.Session.Commands.VerboseLog = Settings.ActivateDebugLog;

                        /// Initialize the session manager.
                        if (SessionManager.Initialize())
                        {
                            IsInitialized = true;
                        }
                    }
                }
                return IsInitialized;
#else
                return true;
#endif
            }

            /// <summary>
            /// Unloads and disables the session manager.
            /// </summary>
            /// <returns>True if the session manager disabled correctly, false otherwise.</returns>
            public bool Terminate()
            {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                // Terminate the session manager.
                if (IsInitialized && SessionManager != null && SessionManager.Terminate())
                {
                    IsInitialized = false;
                }

                GenvidPluginLoader.UnloadSDK();
#endif
                return !IsInitialized;
            }

            /// <summary>
            /// Helper that converts an object to JSON.
            /// </summary>
            /// <param name="data">The object to convert.</param>
            /// <returns>JSON string representation of the provided object.</returns>
            public static String SerializeToJSON(object data)
            {
                var jsonData = JsonUtility.ToJson(data);

                if (jsonData.Equals("{}") && !data.ToString().Equals(""))
                {
                    Debug.LogError(String.Format("JSON serialization failed to handle: {0}", data.ToString()));
                    return null;
                }

                return jsonData;
            }
        }

    }
}