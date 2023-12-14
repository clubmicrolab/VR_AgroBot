using System;
using UnityEngine;
using GenvidSDKCSharp;
using Genvid.Plugin.Listener;

namespace Genvid
{
    namespace Plugin
    {
        /// <summary>
        /// Specifies what parameters to set for an event.
        /// </summary>
        [CreateAssetMenu(fileName = "Genvid Event Item", menuName = "Genvid/Create Event Parameters", order = 3)]
        [Serializable]
        public class GenvidEventParameters : GenvidParametersBase<IGenvidEventListener>
        {
            /// <summary>
            /// Useful function to create an instance of GenvidEventParameters.
            /// ---------------------------------------------------------------
            /// To comply with Unity ScriptableObject instance creation, 
            /// you should not use the new keyword.
            /// Unity ScriptableObject needs to be created using the Unity engine.
            /// If not, Unity will report the warning message:
            /// "ClassName must be instantiated using the ScriptableObject.CreateInstance method instead of new ClassName."
            /// </summary>
            /// <param name="name">The event ID.</param>
            /// <returns></returns>
            public static GenvidEventParameters Create(string name)
            {
                var @event = ScriptableObject.CreateInstance<GenvidEventParameters>();
                @event.Id = name;
                return @event;
            }

            /// <summary>
            /// The event summary: ID, value, and metrics about the event occurence.
            /// </summary>
            public GenvidSDK.EventSummary Summary { get; set; }
            
            /// <summary>
            /// Broadcasts what event was received to all listeners.
            /// </summary>
            public void OnEventReceived()
            {
                foreach (var listener in Listeners)
                {
                    listener.OnEventReceived(this);
                }
            }
        }
    }
}