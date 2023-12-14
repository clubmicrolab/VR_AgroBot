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
            /// <summary>
            /// Container class for callbacks to trigger on stream events.
            /// </summary>
            [Serializable]
            public class StreamEvents
            {
                /// <summary>
                /// Constructor assigning stream events.
                /// </summary>
                /// <param name="OnStart">Callback to call a single time before the first update call.</param>
                /// <param name="OnSubmit">Callback to call with each stream submission request.</param>
                public StreamEvents(UnityAction<string> OnStart, UnityAction<string> OnSubmit)
                {
                    if (OnStart != null)
                    {
                        OnStreamStart = new Stream.Data.StreamEvent();
                        OnStreamStart.AddListener(OnStart);
                    }

                    if (OnSubmit != null)
                    {
                        OnStreamSubmit = new Stream.Data.StreamEvent();
                        OnStreamSubmit.AddListener(OnSubmit);
                    }
                }

                /// <summary>
                /// Callback to call a single time before the first update call.
                /// </summary>
                public Stream.Data.StreamEvent OnStreamStart;

                /// <summary>
                /// Callback to call with each stream submission request.
                /// </summary>
                public Stream.Data.StreamEvent OnStreamSubmit;
            }

            /// <summary>
            /// A data stream listener contains a callback map for what should be done when stream events are triggered.
            /// </summary>
            [Serializable]
            public class StreamListener
            {
                /// <summary>
                /// Constructor for the stream listener.
                /// </summary>
                /// <param name="stream">The stream parameters.</param>
                /// <param name="events">Definition of callbacks for stream events.</param>
                public StreamListener(GenvidStreamParameters stream, StreamEvents events)
                {
                    Stream = stream;
                    Events = events;
                }

                /// <summary>
                /// The stream parameters.
                /// </summary>
                public GenvidStreamParameters Stream;

                /// <summary>
                /// The callbacks to trigger on stream events.
                /// </summary>
                public StreamEvents Events;
            }

            /// <summary>
            /// Specializes the listener base for stream events.
            /// </summary>
            [Serializable]
            public class StreamsListener : GenvidListenerBase<GenvidStreamParameters, StreamListener>, IGenvidStreamListener
            {
                /// <summary>
                /// Registers a stream event-listener within the stream-parameter list of listeners.
                /// </summary>
                /// <param name="listener">The stream event-listener.</param>
                /// <returns>The stream parameters.</returns>
                protected override GenvidStreamParameters RegisterListener(StreamListener listener)
                {
                    if (listener != null)
                    {
                        listener.Stream.RegisterListener(this);
                        return listener.Stream;
                    }

                    return null;
                }

                // <summary>
                /// Removes the stream event-listener from the stream parameter-list of listeners.
                /// </summary>
                /// <param name="listener">The stream event-listener.</param>
                /// <returns>The stream parameters.</returns>
                protected override GenvidStreamParameters UnregisterListener(StreamListener listener)
                {
                    if (listener != null)
                    {
                        listener.Stream.UnregisterListener(this);
                        return listener.Stream;
                    }

                    return null;
                }

                /// <summary>
                /// Invokes the OnStart stream event-listener callback.
                /// Triggered before the first update call.
                /// </summary>
                /// <param name="stream">The stream parameters.</param>
                public void OnStreamStart(GenvidStreamParameters stream)
                {
                    StreamListener listener;
                    if (TryGetValue(stream, out listener))
                    {
                        if (listener.Events.OnStreamStart != null)
                        {
                            listener.Events.OnStreamStart.Invoke(stream.Id);
                        }
                    }
                }

                /// <summary>
                /// Invokes the Submit stream event-listener callback.
                /// Triggered with each submission attempt.
                /// </summary>
                /// <param name="stream">The stream parameters.</param>
                public void OnStreamSubmit(GenvidStreamParameters stream)
                {
                    StreamListener listener;
                    if (TryGetValue(stream, out listener))
                    {
                        if (listener.Events.OnStreamSubmit != null)
                        {
                            listener.Events.OnStreamSubmit.Invoke(stream.Id);
                        }
                    }
                }
            }

            /// <summary>
            /// MonoBehaviour wrapper around the IGenvidStreamListener interface.
            /// </summary>
            [Serializable]
            public class GenvidStreamsListener : MonoBehaviour, IGenvidStreamListener
            {
                /// <summary>
                /// Stream event-listener object containing the list of listeners.
                /// </summary>
                [SerializeField]
                private StreamsListener m_Streams;

                /// <summary>
                /// Returns the stream events-listener container.
                /// </summary>
                public StreamsListener Streams { get { return m_Streams; } }

                /// <summary>
                /// Creates the stream event-listener container on MonoBehaviour instantiation.
                /// </summary>
                private void Awake()
                {
                    if (m_Streams == null)
                    {
                        m_Streams = new StreamsListener();
                    }
                }

                /// <summary>
                /// Initializes the stream container when the MonoBehaviour becomes active.
                /// </summary>
                private void OnEnable()
                {
                    m_Streams.Initialize();
                    m_Streams.EnableListeners();
                }

                /// <summary>
                /// Cleans up the stream container when the MonoBehaviour becomes inactive.
                /// </summary>
                private void OnDisable()
                {
                    m_Streams.DisableListeners();
                    m_Streams.Terminate();
                }

                /// <summary>
                /// Invokes the OnStart stream event-listener callback.
                /// Triggered before the first update call.
                /// </summary>
                /// <param name="stream">The stream parameters.</param>
                public void OnStreamStart(GenvidStreamParameters stream)
                {
                    m_Streams.OnStreamStart(stream);
                }

                /// <summary>
                /// Invokes the Submit stream event-listener callback.
                /// Triggered with each submission attempt.
                /// </summary>
                /// <param name="stream">The stream parameters.</param>
                public void OnStreamSubmit(GenvidStreamParameters stream)
                {
                    m_Streams.OnStreamSubmit(stream);
                }
            }
        }
    }
}