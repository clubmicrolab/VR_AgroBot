using System;
using UnityEngine;
using GenvidSDKCSharp;
using Genvid.Plugin.Listener;

namespace Genvid
{
    namespace Plugin
    {
        /// <summary>
        /// Specifies what parameters to set for a command.
        /// </summary>
        [CreateAssetMenu(fileName = "Genvid Command Item", menuName = "Genvid/Create Command Parameters", order = 4)]
        [Serializable]
        public class GenvidCommandParameters : GenvidParametersBase<IGenvidCommandListener>
        {
            /// <summary>
            /// Useful function to create an instance of GenvidCommandParameters.
            /// ---------------------------------------------------------------
            /// To comply with Unity ScriptableObject instance creation, 
            /// you should not use the new keyword.
            /// Unity ScriptableObject needs to be created using the Unity engine.
            /// If not, Unity will report the warning message:
            /// "ClassName must be instantiated using the ScriptableObject.CreateInstance method instead of new ClassName."
            /// </summary>
            /// <param name="name"> The command ID.</param>
            /// <returns></returns>
            public static GenvidCommandParameters Create(string name)
            {
                var command = ScriptableObject.CreateInstance<GenvidCommandParameters>();
                command.Id = name;
                return command;
            }

            /// <summary>
            /// The command result: ID and value.
            /// </summary>
            public GenvidSDK.CommandResult Result { get; set; }

            /// <summary>
            /// Broadcasts what command was received to all listeners.
            /// </summary>
            public void OnCommandReceived()
            {
                foreach (var listener in Listeners)
                {
                    listener.OnCommandReceived(this);
                }
            }
        }
    }
}