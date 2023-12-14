using GenvidSDKCSharp;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Genvid
{
    namespace Plugin
    {
        namespace Listener
        {
            using GenvidEventAction = UnityAction<string, GenvidSDK.EventResult[], int, IntPtr>;

            /// <summary>
            /// An event listener stores a callback definition for what should be done when
            /// an event is received. The event parameters are used as identifiers.
            /// </summary>
            [Serializable]
            public class EventListener
            {
                /// <summary>
                /// Constructor for the event listener.
                /// </summary>
                /// <param name="@event">The event parameters.</param>
                /// <param name="OnEvent">Unity event to call when the event is received.</param>
                public EventListener(GenvidEventParameters @event, GenvidEventAction OnEvent)
                {
                    Event = @event;

                    if (OnEvent != null)
                    {
                        OnEventTriggered = new GenvidEventType();
                        OnEventTriggered.AddListener(OnEvent);
                    }
                }
                
                /// <summary>
                /// The event parameters.
                /// </summary>
                public GenvidEventParameters Event;

                /// <summary>
                /// Unity event to call when the event is received.
                /// </summary>
                public GenvidEventType OnEventTriggered;
            }

            /// <summary>
            /// Specializes the listener base for events.
            /// </summary>
            [Serializable]
            public class EventsListener : GenvidListenerBase<GenvidEventParameters, EventListener>, IGenvidEventListener
            {
                /// <summary>
                /// Registers an event listener with the event-parameter list of listeners.
                /// </summary>
                /// <param name="listener">The event listener.</param>
                /// <returns>The event parameters.</returns>
                protected override GenvidEventParameters RegisterListener(EventListener listener)
                {
                    if (listener != null)
                    {
                        listener.Event.RegisterListener(this);
                        return listener.Event;
                    }

                    return null;
                }

                /// <summary>
                /// Removes the event listener from the event parameters.
                /// </summary>
                /// <param name="listener">The event listener.</param>
                /// <returns>The event parameters.</returns>
                protected override GenvidEventParameters UnregisterListener(EventListener listener)
                {
                    if (listener != null)
                    {
                        listener.Event.UnregisterListener(this);
                        return listener.Event;
                    }

                    return null;
                }

                /// <summary>
                /// Invokes the event callback defined in the listener.
                /// Used on receiving an event.
                /// </summary>
                /// <param name="@event">The event parameters.</param>
                public void OnEventReceived(GenvidEventParameters @event)
                {
                    EventListener listener;
                    if (TryGetValue(@event, out listener))
                    {
                        if (listener.OnEventTriggered != null)
                        {
                            listener.OnEventTriggered.Invoke(@event.Summary.id, @event.Summary.results,
                                                             @event.Summary.numResults, @event.UserData);
                        }
                    }
                }
            }

            /// <summary>
            /// MonoBehaviour wrapper around the IGenvidEventListener interface.
            /// </summary>
            public class GenvidEventsListener : MonoBehaviour, IGenvidEventListener
            {
                /// <summary>
                /// Event-listener object containing the list of event listeners.
                /// </summary>
                [SerializeField]
                private EventsListener m_Events;

                /// <summary>
                /// Returns the event-listener container.
                /// </summary>
                public EventsListener Events { get { return m_Events; } }

                /// <summary>
                /// Creates the event container on MonoBehaviour instantiation.
                /// </summary>
                private void Awake()
                {
                    if (m_Events == null)
                    {
                        m_Events = new EventsListener();
                    }
                }

                /// <summary>
                /// Initializes the event container when the MonoBehaviour becomes active.
                /// </summary>
                private void OnEnable()
                {
                    m_Events.Initialize();
                    m_Events.EnableListeners();
                }

                /// <summary>
                /// Cleans up the event container when the MonoBehaviour becomes inactive.
                /// </summary>
                private void OnDisable()
                {
                    m_Events.DisableListeners();
                    m_Events.Terminate();
                }

                /// <summary>
                /// Invokes the event callback defined in the listener.
                /// Used on receiving an event.
                /// </summary>
                /// <param name="@event">The event parameters.</param>
                public void OnEventReceived(GenvidEventParameters @event)
                {
                    m_Events.OnEventReceived(@event);
                }
            }
        }
    }
}