using System;
using System.Collections.Generic;
using Genvid;
using GenvidSDKCSharp;
using UnityEngine;
using UnityEngine.Events;

namespace Genvid
{
    /// <summary>
    /// Class representing a command event as a Unity event.
    /// </summary>
    [Serializable]
    public class CommandEvent : UnityEvent<string, string, IntPtr> { }

    /// <summary>
    /// Class representing a command.
    /// </summary>
    [Serializable]
    public class CommandElement
    {
        public string Id;
        public CommandEvent OnCommandTriggered;
    }

    namespace Plugin
    {
        namespace Channel
        {
            /// <summary>
            /// Specializes the Genvid checker for commands.
            /// </summary>
            [Serializable]
            public class Commands : GenvidChecker<GenvidSDK.CommandResult, GenvidCommandParameters>
            {
                /// <summary>
                /// List of command parameters.
                /// </summary>
                public List<GenvidCommandParameters> Settings;

#if !(UNITY_EDITOR || UNITY_STANDALONE_WIN)
            // Disable warning for other platforms.
#pragma warning disable 414
#endif
                /// <summary>
                /// Defines what to do when a command is received.
                /// </summary>
                private GenvidSDK.CommandCallback m_CommandCallback = null;

#if !(UNITY_EDITOR || UNITY_STANDALONE_WIN)
#pragma warning restore 414
#endif
                /// <summary>
                /// The string name of this class. Used in logs.
                /// </summary>
                protected override string Typename { get { return "Command"; } }

                /// <summary>
                /// Initializes the parameter list for the GenvidChecker.
                /// Called before the checker iterates on the parameters.
                /// </summary>
                protected override void Init()
                {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                    m_CommandCallback = new GenvidSDK.CommandCallback(CommandCallbackFunction);
                    m_SubscribedIds = Settings;
#endif
                }

                /// <summary>
                /// Cleans up command-specific members.
                /// Done after the checker finishes unsubscribing the commands.
                /// </summary>
                protected override void Term()
                {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                    m_CommandCallback = null;
                    m_SubscribedIds = null;
#endif
                }

                /// <summary>
                /// Subscribes a command.
                /// </summary>
                /// <param name="id">ID of the command.</param>
                /// <param name="userData">The received data concerning the command.</param>
                /// <returns>The operation status. GenvidSDK.success if everything went well.</returns>
                protected override GenvidSDK.Status SubscribeImpl(string id, IntPtr userData)
                {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                    return GenvidSDK.SubscribeCommand(id, m_CommandCallback, userData);
#else
                return GenvidSDK.Status.Success; 
#endif
                }

                /// <summary>
                /// Unsubscribes a command.
                /// </summary>
                /// <param name="id">ID of the command.</param>
                /// <param name="userData">The user data concerning the command.</param>
                /// <returns>The operation status. GenvidSDK.success if everything went well.</returns>
                protected override GenvidSDK.Status UnsubscribeImpl(string id, IntPtr userData)
                {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                    return GenvidSDK.UnsubscribeCommand(id, m_CommandCallback, userData);
#else
                return GenvidSDK.Status.Success;
#endif
                }

                /// <summary>
                /// Used to invoke the command-reception callback when a new command is processed by the checker.
                /// </summary>
                /// <param name="dataEvent">Container with the parameters and received data.</param>
                protected override void OnInvokeFunction(DataFunction<GenvidCommandParameters, GenvidSDK.CommandResult> dataEvent)
                {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                    dataEvent.@event.Result = dataEvent.Data;
                    dataEvent.@event.UserData = dataEvent.UserData;
                    dataEvent.@event.OnCommandReceived();
#endif
                }

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                /// <summary>
                /// Add an command to the list of received commands.
                /// </summary>
                /// <param name="result">The command result.</param>
                /// <param name="userData">The user data associated with the command.</param>
                private void CommandCallbackFunction(GenvidSDK.CommandResult result, IntPtr userData)
                {
                        try
                        {
                            PushData(result.id, result, userData);
                        }
                        catch (Exception e)
                        {
                            Debug.LogError("Exception during Command Callback: " + e.ToString());
                        }
                }
#endif
            }
        }
    }
}