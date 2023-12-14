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
            using GenvidCommandAction = UnityAction<string, string, IntPtr>;

            /// <summary>
            /// A command listener stores a callback definition for what should be done when
            /// a command is received. The command parameters are used as identifiers.
            /// </summary>
            [Serializable]
            public class CommandListener
            {
                /// <summary>
                /// Constructor for the command listener.
                /// </summary>
                /// <param name="command">The command parameters.</param>
                /// <param name="OnCommand">Unity event to call when the command is received.</param>
                public CommandListener(GenvidCommandParameters command, GenvidCommandAction OnCommand)
                {
                    Command = command;
                    if (OnCommand != null)
                    {
                        OnCommandTriggered = new CommandEvent();
                        OnCommandTriggered.AddListener(OnCommand);
                    }
                }

                /// <summary>
                /// The command parameters.
                /// </summary>
                public GenvidCommandParameters Command;

                /// <summary>
                /// Unity event to call when the command is received.
                /// </summary>
                public CommandEvent OnCommandTriggered;
            }

            /// <summary>
            /// Specializes the listener base for commands.
            /// </summary>
            [Serializable]
            public class CommandsListener : GenvidListenerBase<GenvidCommandParameters, CommandListener>, IGenvidCommandListener
            {
                /// <summary>
                /// Registers a command listener with the command-parameter list of listeners.
                /// </summary>
                /// <param name="listener">The command listener.</param>
                /// <returns>The command parameters.</returns>
                protected override GenvidCommandParameters RegisterListener(CommandListener listener)
                {
                    if(listener != null)
                    {
                        listener.Command.RegisterListener(this);
                        return listener.Command;
                    }

                    return null;
                }

                /// <summary>
                /// Removes the command listener from the command parameters.
                /// </summary>
                /// <param name="listener">The command listener.</param>
                /// <returns>The command parameters.</returns>
                protected override GenvidCommandParameters UnregisterListener(CommandListener listener)
                {
                    if (listener != null)
                    {
                        listener.Command.UnregisterListener(this);
                        return listener.Command;
                    }

                    return null;
                }

                /// <summary>
                /// Invokes the command callback defined in the listener.
                /// Used on receiving a command.
                /// </summary>
                /// <param name="commandParams">The command parameters.</param>
                public void OnCommandReceived(GenvidCommandParameters commandParams)
                {
                    CommandListener listener;
                    if (TryGetValue(commandParams, out listener))
                    {
                        if (listener.OnCommandTriggered != null)
                        {
                            listener.OnCommandTriggered.Invoke(commandParams.Result.id, commandParams.Result.value, commandParams.UserData);
                        }
                    }
                }
            }

            /// <summary>
            /// MonoBehavior wrapper around the IGenvidCommandListener interface.
            /// </summary>
            public class GenvidCommandsListener : MonoBehaviour, IGenvidCommandListener
            {
                /// <summary>
                /// Command-listener object containing the list of command listeners.
                /// </summary>
                [SerializeField]
                private CommandsListener m_Commands;

                /// <summary>
                /// Returns the command-listener container.
                /// </summary>
                public CommandsListener Commands { get { return m_Commands; } }

                /// <summary>
                /// Creates the commands container on MonoBehaviour instantiation.
                /// </summary>
                private void Awake()
                {
                    if (m_Commands == null)
                    {
                        m_Commands = new CommandsListener();
                    }
                }

                /// <summary>
                /// Initializes the commands container when the MonoBehaviour becomes active.
                /// </summary>
                private void OnEnable()
                {
                    m_Commands.Initialize();
                    m_Commands.EnableListeners();
                }

                /// <summary>
                /// Cleans up the commands container when the MonoBehaviour becomes inactive.
                /// </summary>
                private void OnDisable()
                {
                    m_Commands.DisableListeners();
                    m_Commands.Terminate();
                }

                /// <summary>
                /// Invokes the command callback defined in the listener.
                /// Used on receiving a command.
                /// </summary>
                /// <param name="commandParams">The command parameters.</param>
                public void OnCommandReceived(GenvidCommandParameters commandParams)
                {
                    m_Commands.OnCommandReceived(commandParams);
                }
            }
        }
    }
}