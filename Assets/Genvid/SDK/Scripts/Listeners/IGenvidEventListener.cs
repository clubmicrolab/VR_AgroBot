using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Genvid
{
    namespace Plugin
    {
        namespace Listener
        {
            /// <summary>
            /// Interface specification for stream listeners.
            /// </summary>
            public interface IGenvidEventListener : IGenvidListenerBase
            {
                /// <summary>
                /// Invokes the event callback defined in the listener.
                /// Used on receiving an event.
                /// </summary>
                /// <param name="eventParams">The event parameters.</param>
                void OnEventReceived(GenvidEventParameters eventParams);
            }
        }
    }
}