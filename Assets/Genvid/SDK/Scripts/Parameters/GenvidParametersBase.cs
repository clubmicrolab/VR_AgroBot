using System;
using System.Collections.Generic;
using Genvid.Plugin.Listener;
using UnityEngine;
using UnityEngine.Events;

namespace Genvid
{
    namespace Plugin
    {
        /// <summary>
        /// The user can use parameters to define streams, events, commands, etc.
        /// </summary>
        public abstract class AGenvidParametersBase : ScriptableObject
        {
            /// <summary>
            /// Unique identifier.
            /// </summary>
            public string Id;

            /// <summary>
            /// Stores any data needed by listeners.
            /// </summary>
            public IntPtr UserData { get; set; }

            /// <summary>
            /// Are there any registered listeners?
            /// </summary>
            public virtual bool HasRegisteredListeners { get; protected set; }
        }

        /// <summary>
        /// Templated derivation of AGenvidParametersBase that handles listeners.
        /// </summary>
        public abstract class GenvidParametersBase<T> : AGenvidParametersBase
        {
            /// <summary>
            /// Listener list.
            /// </summary>
            private readonly List<T> m_Listeners = new List<T>();

            /// <summary>
            /// Listener list getter.
            /// </summary>
            protected List<T> Listeners { get { return m_Listeners; } }

            /// <summary>
            /// Are there any registered listeners?
            /// </summary>
            public override bool HasRegisteredListeners { get { return m_Listeners.Count > 0; } }

            /// <summary>
            /// Registers a listener.
            /// </summary>
            /// <param name="listener">The listener to add to the list.</param>
            public void RegisterListener(T listener)
            {
                if (!m_Listeners.Contains(listener))
                {
                    m_Listeners.Add(listener);
                }
            }

            /// <summary>
            /// Removes a listener.
            /// </summary>
            /// <param name="listener">The listener to remove from the list.</param>
            public void UnregisterListener(T listener)
            {
                if (m_Listeners.Contains(listener))
                {
                    m_Listeners.Remove(listener);
                }
            }
        }
    }
}