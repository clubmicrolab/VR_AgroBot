namespace Genvid
{
    namespace Plugin
    {
        namespace Listener
        {
            /// <summary>
            /// Common interface for all listeners.
            /// </summary>
            public interface IGenvidListenerBase
            {
            }

            /// <summary>
            /// Interface specification for stream listeners.
            /// </summary>
            public interface IGenvidStreamListener : IGenvidListenerBase
            {
                /// <summary>
                // Invokes the OnStart stream event-listener callback.
                /// Triggered before the first update call.
                /// </summary>
                /// <param name="stream">The stream parameters.</param>
                void OnStreamStart(GenvidStreamParameters stream);

                /// <summary>
                /// Invokes the Submit stream event-listener callback.
                /// Triggered with each submission attempt.
                /// </summary>
                /// <param name="stream">The stream parameters.</param>
                void OnStreamSubmit(GenvidStreamParameters stream);
            }
        }
    }
}