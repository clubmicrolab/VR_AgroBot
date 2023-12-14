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
            /// Interface specification for command listeners.
            /// </summary>
            public interface IGenvidCommandListener : IGenvidListenerBase
            {
                /// <summary>
                /// Invokes the command callback defined in the listener.
                /// Used on receiving a command.
                /// </summary>
                /// <param name="commandParams">The command parameters.</param>
                void OnCommandReceived(GenvidCommandParameters commandParams);
            }
        }
    }
}