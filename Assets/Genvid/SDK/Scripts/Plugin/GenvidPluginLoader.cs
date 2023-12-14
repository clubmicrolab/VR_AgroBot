using GenvidSDKCSharp;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genvid
{
    namespace Plugin
    {
        /// <summary>
        /// Helper class that handles loading/unloading the Genvid dynamically linked library.
        /// </summary>
        public static class GenvidPluginLoader
        {
            /// <summary>
            /// Keeps track of whether or not the Genvid library was loaded.
            /// </summary>
            public static bool IsLoaded { get; private set; }

            /// <summary>
            /// Loads the Genvid library using the application root path.
            /// </summary>
            /// <returns>True if successfully loaded, false otherwise.</returns>
            public static bool LoadSDK()
            {
                return LoadSDK(null);
            }

            /// <summary>
            /// Loads the Genvid library.
            /// </summary>
            /// <param name="rootDataPath">Path to the project housing the DLL. The application folder is used if none is specified.</param>
            /// <returns>True if successfully loaded, false otherwise.</returns>
            public static bool LoadSDK(string rootDataPath)
            {
                bool result = false;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                if (IsLoaded)
                {
                    return IsLoaded;
                }

                if (rootDataPath == null || rootDataPath.Length == 0)
                {
                    rootDataPath = Application.dataPath;
                }

                /// 32 bits -> x86
                /// 64 bits -> x86_64

                string dllPath = "/Plugins";
                string editorDllPath = "/Genvid/SDK/Plugins";
                string[] paths = { "", "/x86" };
                bool arch86_64 = (IntPtr.Size == 8);
                

                for (int i = 0; i < paths.Length; i++)
                {
                    if (Application.isEditor)
                    {
                        dllPath = editorDllPath + (arch86_64 ? "/x64" : "/x86");
                    }
                    else if (paths[i].Length != 0)
                    {
                        dllPath += arch86_64 ? (paths[i] + "_64") : paths[i];
                    }

                    if (GenvidSDK.LoadGenvidDll(rootDataPath + dllPath))
                    {
                        Debug.Log("genvid.dll successfully loaded from " + rootDataPath + dllPath);
                        result = true;
                        break;
                    }
                }

                if (!result)
                {
                    Debug.LogError("Failed to load genvid.dll from " + rootDataPath);
                    result = false;
                }
#endif
                IsLoaded = result;
                return result;
            }

            /// <summary>
            /// Unloads the Genvid library.
            /// </summary>
            public static void UnloadSDK()
            {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                if (IsLoaded)
                {
                    GenvidSDK.UnloadGenvidDll();
                    Debug.Log("genvid.dll successfully unloaded.");
                    IsLoaded = false;
                }
#endif
            }
        }
    }
}