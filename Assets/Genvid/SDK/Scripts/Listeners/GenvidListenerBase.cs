using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Genvid
{
    namespace Plugin
    {
        namespace Listener
        {
            /// <summary>
            /// Class handling listener caches.
            /// </summary>
            [Serializable]
            public abstract class GenvidListenerBase<T, U> : IGenvidListenerBase
            {
                /// <summary>
                /// List of listeners.
                /// </summary>
                [SerializeField]
                private List<U> m_Listeners;

                /// <summary>
                /// Stores added listeners.
                /// Key is defined by the base class. Usually a derived class of Genvid.Plugin.AGenvidParametersBase.
                /// Value is defined by the derived class. Usually a series of events.
                /// </summary>
                private Dictionary<T, U> m_Map;

                /// <summary>
                /// Retrieves the listener list.
                /// </summary>
                public List<U> Listeners { get { return m_Listeners; } }

                protected abstract T RegisterListener(U listener);
                protected abstract T UnregisterListener(U listener);

                /// <summary>
                /// Creates the containers on initialization.
                /// </summary>
                public void Initialize()
                {
                    m_Map = new Dictionary<T, U>();
                    if (m_Listeners == null)
                    {
                        m_Listeners = new List<U>();
                    }
                }

                /// <summary>
                /// Cleans up the containers.
                /// </summary>
                public void Terminate()
                {
                    m_Listeners.Clear();
                    m_Map.Clear();
                }

                /// <summary>
                /// Adds a listener to the list.
                /// </summary>
                /// <param name="containerListener">The listener to add.</param>
                public void AddListener(U containerListener)
                {
                    m_Listeners.Add(containerListener);
                }

                /// <summary>
                /// Removes a listener from the list.
                /// </summary>
                /// <param name="containerListener">The listener to remove.</param>
                public void RemoveListener(U containerListener)
                {
                    m_Listeners.Remove(containerListener);
                }

                /// <summary>
                /// Registers known listeners.
                /// Stores listeners according to the returned registration key.
                /// Registration key is defined by the derived classes.
                /// </summary>
                public void EnableListeners()
                {
                    foreach (var listener in m_Listeners)
                    {
                        var param = RegisterListener(listener);
                        if (param != null && listener != null)
                        {
                            m_Map.Add(param, listener);
                        }
                    }
                }

                /// <summary>
                /// Removes all listeners from the cache.
                /// </summary>
                public void DisableListeners()
                {
                    foreach (var listener in m_Listeners)
                    {
                        var param = UnregisterListener(listener);
                        if (param != null)
                        {
                            m_Map.Remove(param);
                        }
                    }
                }

                /// <summary>
                /// Retrieves key and listener from the cache if the entry exists.
                /// </summary>
                protected bool TryGetValue(T item, out U listener)
                {
                    return m_Map.TryGetValue(item, out listener);
                }
            }
        }
    }
}