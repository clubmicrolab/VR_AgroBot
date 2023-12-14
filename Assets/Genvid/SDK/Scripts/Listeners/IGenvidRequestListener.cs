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
            /// Interface specification for request listeners.
            /// </summary>
            public interface IGenvidRequestListener : IGenvidListenerBase
            {
                /// <summary>
                /// Invokes the request callback defined in the listener.
                /// Used on receiving a request.
                /// </summary>
                /// <param name="requestParams">The request parameters.</param>
                void OnRequestReceived(GenvidRequestParameters requestParams);
            }
        }
    }
}