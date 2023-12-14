using System;
using UnityEngine;
using GenvidSDKCSharp;
using Genvid.Plugin.Listener;

namespace Genvid
{
    namespace Plugin
    {
        /// <summary>
        /// Specifies what parameters to set for a request.
        /// </summary>
        [CreateAssetMenu(fileName = "Genvid Request Item", menuName = "Genvid/Create Request Parameters", order = 4)]
        [Serializable]
        public class GenvidRequestParameters : GenvidParametersBase<IGenvidRequestListener>
        {
            /// <summary>
            /// Useful function to create an instance of GenvidRequestParameters.
            /// ---------------------------------------------------------------
            /// To comply with Unity ScriptableObject instance creation, 
            /// you should not use the new keyword.
            /// Unity ScriptableObject needs to be created using the Unity engine.
            /// If not, Unity will report the warning message:
            /// "ClassName must be instantiated using the ScriptableObject.CreateInstance method instead of new ClassName."
            /// </summary>
            /// <param name="topic"> The request topic.</param>
            /// <returns></returns>
            public static GenvidRequestParameters Create(string topic)
            {
                var request = ScriptableObject.CreateInstance<GenvidRequestParameters>();
                request.Id = topic;
                return request;
            }

            /// <summary>
            /// The request received: Id and value.
            /// </summary>
            public GenvidSDK.RequestResult Result { get; set; }

            /// <summary>
            /// Broadcasts what request was received to all listeners.
            /// </summary>
            public void OnRequestReceived()
            {
                foreach (var listener in Listeners)
                {
                    listener.OnRequestReceived(this);
                }
            }
        }
    }
}