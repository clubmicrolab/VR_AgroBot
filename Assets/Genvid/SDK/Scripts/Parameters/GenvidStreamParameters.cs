using System;
using UnityEngine;
using Genvid.Plugin.Listener;

namespace Genvid
{
    namespace Plugin
    {
        /// <summary>
        /// Specifies what parameters to set for a data stream.
        /// </summary>
        [CreateAssetMenu(fileName = "Genvid Stream Item", menuName = "Genvid/Create Stream Parameters", order = 2)]
        [Serializable]
        public class GenvidStreamParameters : GenvidParametersBase<IGenvidStreamListener>
        {
            /// <summary>
            /// Maintains a periodic deadline against the frame time.
            /// </summary>
            private PeriodicDeadline m_Deadline = new PeriodicDeadline();

            /// <summary>
            /// The periodic deadline keeps the stream from submitting more frequently than the framerate.
            /// </summary>
            public PeriodicDeadline Deadline
            {
                get
                {
                    /// Ensure the framerate is consistent.
                    m_Deadline.Framerate = Framerate;
                    return m_Deadline;
                }
            }

            /// <summary>
            /// Useful function to create an instance of GenvidStramParameters.
            /// ---------------------------------------------------------------
            /// To comply with Unity ScriptableObject instance creation, 
            /// you should not use the new keyword.
            /// Unity ScriptableObject needs to be created using the Unity engine.
            /// If not, Unity will report the warning message:
            /// "ClassName must be instantiated using the ScriptableObject.CreateInstance method instead of new ClassName."
            /// </summary>
            /// <param name="name"></param>
            /// <param name="framerate"></param>
            /// <returns></returns>
            public static GenvidStreamParameters Create(string name, float framerate = 30.0f)
            {
                var stream = ScriptableObject.CreateInstance<GenvidStreamParameters>();
                stream.Id = name;
                stream.Framerate = framerate;
                return stream;
            }

            /// <summary>
            /// The stream framerate.
            /// </summary>
            [Range(0.001f, 60.0f)]
            [Tooltip("Stream framerate")]
            public float Framerate = 30f;

            /// <summary>
            /// Broadcasts the session start-event to all listenrs.
            /// </summary>
            public void OnStart()
            {
                foreach (var listener in Listeners)
                {
                    listener.OnStreamStart(this);
                }
            }

            /// <summary>
            /// Broadcasts to all listenres when data is submitted on the stream.
            /// </summary>
            public void OnSubmitStream()
            {
                foreach (var listener in Listeners)
                {
                    listener.OnStreamSubmit(this);
                }
            }
        }
    }
}