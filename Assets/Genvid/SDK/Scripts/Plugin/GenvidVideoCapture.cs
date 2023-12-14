using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;
using GenvidSDKCSharp;
using UnityEngine.Rendering;

namespace Genvid
{
    namespace Plugin
    {
        /// <summary>
        /// Helper class exposing video-related functions from the GenvidPlugin library.
        /// </summary> 
        public class GenvidPluginDLL
        {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            // GENVID - Start DLL import

            /// <summary>
            /// Finds out if the video resource was successfully assigned
            /// in the Genvid DLL during a render initialization event.
            /// </summary>
            /// <returns>GenvidStatus_Success if the resource was successfullly assigned, another status otherwise.</returns>
            [DllImport("GenvidPlugin")]
            public static extern GenvidSDK.Status GetVideoInitStatus();

            /// <summary>
            /// Finds out if the Genvid DLL frame grabber successfully auto-captured and submitted a frame.
            /// </summary>
            /// <returns>GenvidStatus_Success if the submit was successful, another status otherwise.</returns>
            [DllImport("GenvidPlugin")]
            public static extern GenvidSDK.Status GetVideoSubmitDataStatus();

            /// <summary>
            /// Set the video channel name (also called the stream ID).
            /// </summary>
            [DllImport("GenvidPlugin")]
            public static extern void SetupVideoChannel(string streamID);

            /// <summary>
            /// Invalidate the reference to the graphics interface.
            /// Done automatically following a successful render event.
            /// </summary>
            [DllImport("GenvidPlugin")]
            public static extern void CleanUp();

            /// <summary>
            /// Retrieve the render event function.
            /// </summary>
            [DllImport("GenvidPlugin")]
            public static extern IntPtr GetRenderEventFunc();
            // GENVID - Stop DLL import
#endif
        }

        /// <summary>
        /// Helper class facilitating usage of automatic video-capture through the DLL,
        /// as well as manual capture through Unity camera objects.
        /// </summary> 
        public class GenvidVideoCapture
        {
            /// <summary>
            /// The video stream ID.
            /// </summary>
            public string StreamName { get; private set; }

            /// <summary>
            /// The video framerate.
            /// </summary>
            public float Framerate { get; private set; }

            /// <summary>
            /// Specifies whether to use the Genvid DLL auto-capture or 
            /// a Unity camera object.
            /// </summary>
            public GenvidVideoParameters.eCaptureType CaptureType { get; private set; }

            /// <summary>
            /// Assign the new video source here. Detects if we have a new video source.
            /// </summary>
            public UnityEngine.Object VideoSource { get; set; }

#if !(UNITY_EDITOR || UNITY_STANDALONE_WIN)
            // Disable warning for other platforms.
            #pragma warning disable 414
#endif
            /// <summary>
            /// The video source necessary for non-automatic mode: Camera or Containing Camera.
            /// </summary>
            private UnityEngine.Object m_CurrentVideoSource;

            /// <summary>
            /// Object used to hold temporary texture space while the camera is in the process of rendering.
            /// </summary>
            private RenderTexture m_RenderTexture;

            /// <summary>
            /// The texture reference.
            /// </summary>
            private IntPtr m_TexturePtr;

            /// <summary>
            /// True if the video submit coroutine has ended.
            /// </summary>
            private bool m_TerminateCoroutine = false;

            /// <summary>
            /// Used in the video-submission coroutine to track if the video-submission process is initialised.
            /// </summary>
            private bool m_ProcessComplete = true;

            /// <summary>
            /// Toggle to true to exit the video submit coroutine.
            /// </summary>
            private bool m_QuitProcess = false;

            /// <summary>
            /// List of graphics commands to execute.
            /// Used here to tell the camera to copy the resulting texture into a predefined buffer once it's done rendering.
            /// </summary>
            private CommandBuffer m_CommandBuffer;

            /// <summary>
            /// Command buffer name. Used in debugging.
            /// </summary>
            private const string m_CommandBufferName = "MultipleCapture";

#if !(UNITY_EDITOR || UNITY_STANDALONE_WIN)
            #pragma warning restore 414
#endif

            /// <summary>
            /// Specifies logging verbosity. Toggle on for more logging info.
            /// </summary>
            public bool VerboseLog { get; set; }

            /// <summary>
            /// Initialises the video stream parameters.
            /// </summary>
            /// <param name="streamName">ID of the video stream.</param>
            /// <param name="framerate">The stream framerate.</param>
            /// <param name="captureType">The capture type: Auto or Texture.</param>
            /// <param name="videoSource">Specifies the video source.</param>
            public GenvidVideoCapture(string streamName, float framerate, GenvidVideoParameters.eCaptureType captureType, UnityEngine.Object videoSource)
            {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                StreamName = streamName;
                Framerate = framerate;
                CaptureType = captureType;
                VideoSource = videoSource;
                Plugin.GenvidPluginDLL.SetupVideoChannel(streamName);
#endif
            }

            /// <summary>
            /// Lets the video capture coroutine terminate.
            /// </summary>
            public void QuitProcess()
            {
                m_QuitProcess = true;
            }

            /// <summary>
            /// Destroys and invalidates resources used for video capture.
            /// </summary>
            public void CleanupResources()
            {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                if (GetCameraFromSource(m_CurrentVideoSource) != null)
                {
                    CleanupCameraCommandBuffer(m_CurrentVideoSource);

                    if (m_CommandBuffer != null)
                    {
                        m_CommandBuffer.Clear();
                    }
                    DestroyTexture();
                }

                GenvidPluginDLL.CleanUp();
#endif
            }

            /// <summary>
            /// Waits for the video capture coroutine to exit if the session manager is not
            /// currently terminating the session. Then cleans up video capture resources.
            /// </summary>
            /// <returns>Returns a wait-until object for the end of the video capture coroutine.</returns>
            public IEnumerator DestroyVideo(bool isCurrentlyDestroying)
            {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                // Wait only if the application does not quit.
                if (!isCurrentlyDestroying)
                {
                    yield return new WaitUntil(() => m_TerminateCoroutine == true);
                }
                CleanupResources();
#else
                yield return null;
#endif
            }

            /// <summary>
            /// Releases the member texture.
            /// </summary>
            public void DestroyTexture()
            {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                if (m_RenderTexture != null)
                {
                    m_RenderTexture.Release();
                    UnityEngine.Object.Destroy(m_RenderTexture);
                }
#endif
            }

            /// <summary>
            /// Retrieves the camera's texture if available.
            /// If the texture isn't rendered yet, we create a temporary buffer to which
            /// the texture will be assigned once rendered.
            /// Use this to manually retrieve the camera's texture if
            /// auto-capture mode isn't enabled.
            /// </summary>
            /// <param name="camera">The camera object.</param>
            public void CaptureCamera(Camera camera)
            {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                DestroyTexture();

                if (camera.targetTexture != null)
                {
                    m_TexturePtr = camera.targetTexture.GetNativeTexturePtr();

                    var gvStatus = GenvidSDK.SetParameter(StreamName, "video.useopenglconvention", 1);
                    if (GenvidSDK.StatusFailed(gvStatus))
                    {
                        Debug.LogError("Error while setting the OpenGL convention: " + GenvidSDK.StatusToString(gvStatus));
                    }
                    else if (VerboseLog)
                    {
                        Debug.Log("Genvid SetParameter OpenGL performed correctly.");
                    }
                    Debug.Log("Your camera is using a render texture. Using the render texture instead of the camera for the video stream.");
                }
                else
                {
                    m_RenderTexture = new RenderTexture(camera.pixelWidth, camera.pixelHeight, 24, RenderTextureFormat.ARGB32);
                    if (m_RenderTexture.Create())
                    {
                        m_TexturePtr = m_RenderTexture.GetNativeTexturePtr();

                        m_CommandBuffer = new CommandBuffer();
                        m_CommandBuffer.name = m_CommandBufferName;
                        camera.AddCommandBuffer(CameraEvent.AfterEverything, m_CommandBuffer);
                        m_CommandBuffer.Blit(BuiltinRenderTextureType.CurrentActive, m_RenderTexture);
                    }
                    else
                    {
                        Debug.LogError("Failed to create the render texture.");
                    }
                }
#endif
            }

            // GENVID - Video capture start

            /// <summary>
            /// Starts a coroutine that submits video frames when the camera is done rendering.
            /// </summary>
            /// <param name="deadline">Periodic deadline object used to limit the frame submissions to the stream framerate.</param>
            /// <param name="disableVideoDataSubmissionThrottling">False to limit the frame submissions to the stream framerate. True to submit as fast as Unity can render.</param>
            /// <returns>True if a new video source is detected, false otherwise.</returns>
            public IEnumerator CallPluginAtEndOfFrames(PeriodicDeadline deadline, bool disableVideoDataSubmissionThrottling)
            {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                /// GetRenderEventFunc parameter.
                System.IntPtr renderingFunction = Plugin.GenvidPluginDLL.GetRenderEventFunc();
                var waitForEndOfFrame = new WaitForEndOfFrame();
                GenvidSDK.Status status = GenvidSDK.Status.Success;

                while (true)
                {
                    /// Wait until all frame rendering is done.
                    yield return waitForEndOfFrame;

                    if (m_QuitProcess == false)
                    {
                        if (m_ProcessComplete)
                        {
                            if (OnRenderEventInit())
                            {
                                if (CaptureType != GenvidVideoParameters.eCaptureType.Automatic)
                                {
                                    status = GenvidSDK.SetParameterPointer(StreamName, "video.source.id3d11texture2d", m_TexturePtr);
                                    if (GenvidSDK.StatusFailed(status))
                                    {
                                        Debug.LogError("Error while initializing texture for capture: " + GenvidSDK.StatusToString(status));
                                        if (status == GenvidSDK.Status.InvalidParameter)
                                        {
                                            Debug.LogError("Make sure you are using a D3D11 graphic driver.");
                                        }
                                    }
                                    else if (VerboseLog)
                                    {
                                        Debug.Log("Genvid SetParameterPointer for texture2d performed correctly.");
                                    }
                                }
                                else
                                {
                                    GL.IssuePluginEvent(renderingFunction, 0);
                                    status = Plugin.GenvidPluginDLL.GetVideoInitStatus();
                                    if (GenvidSDK.StatusFailed(status))
                                    {
                                        Debug.LogError("Error while starting the video stream : " + GenvidSDK.StatusToString(status));
                                        if (status == GenvidSDK.Status.InvalidParameter)
                                        {
                                            Debug.LogError("Make sure you're using a D3D11 graphic driver.");
                                        }
                                    }
                                    else if (VerboseLog)
                                    {
                                        Debug.Log("Genvid GetVideoInitStatus performed correctly.");
                                    }
                                    status = GenvidSDK.Status.ConnectionInProgress;
                                }
                            }
                            else
                            {
                                GL.IssuePluginEvent(renderingFunction, 0);
                                status = Plugin.GenvidPluginDLL.GetVideoInitStatus();
                                if (GenvidSDK.StatusFailed(status))
                                {
                                    Debug.LogError("Error while starting the video stream : " + GenvidSDK.StatusToString(status));
                                    if (status == GenvidSDK.Status.InvalidParameter)
                                    {
                                        Debug.LogError("Make sure you're using a D3D11 graphic driver.");
                                    }
                                }
                                else if (VerboseLog)
                                {
                                    Debug.Log("Genvid GetVideoInitStatus performed correctly.");
                                }
                                status = GenvidSDK.Status.ConnectionInProgress;
                            }
                            m_ProcessComplete = false;
                        }
                        else
                        {
                            if (OnRenderEventUpdate())
                            {
                                if (CaptureType != GenvidVideoParameters.eCaptureType.Automatic)
                                {
                                    status = GenvidSDK.SetParameterPointer(StreamName, "video.source.id3d11texture2d", m_TexturePtr);
                                    if (GenvidSDK.StatusFailed(status))
                                    {
                                        Debug.LogError("Error while assigning new texture for capture: " + GenvidSDK.StatusToString(status));
                                        if (status == GenvidSDK.Status.InvalidParameter)
                                        {
                                            Debug.LogError("Make sure you're using a D3D11 graphic driver.");
                                        }
                                    }
                                    else if (VerboseLog)
                                    {
                                        Debug.Log("Genvid SetParameterPointer for texture2d performed correctly.");
                                    }
                                }
                            }

                            if (disableVideoDataSubmissionThrottling)
                            {
                                SubmitVideo(renderingFunction);
                            }
                            else if (deadline.IsPassed())
                            {
                                SubmitVideo(renderingFunction);
                                deadline.Next();
                            }
                        }
                    }
                    else
                    {
                        m_TerminateCoroutine = true;
                        yield break;
                    }
                }
#else
                yield return null;
#endif
            }
            // GENVID - Video capture end

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            /// <summary>
            /// Uses the Genvid library's auto-capture functionality to submit a video frame.
            /// </summary>
            /// <returns>True if a new video source is detected, false otherwise.</returns>
            private GenvidSDK.Status SubmitVideo(System.IntPtr RenderingFunction)
            {
                GL.IssuePluginEvent(RenderingFunction, 1);
                var status = Plugin.GenvidPluginDLL.GetVideoSubmitDataStatus();
                if (GenvidSDK.StatusFailed(status))
                {
                    Debug.LogError("Error while sending video data: " + GenvidSDK.StatusToString(status));
                }
                else if (VerboseLog)
                {
                    Debug.Log("Genvid GetVideoSubmitDataStatus performed correctly.");
                }

                return status;
            }

            /// <summary>
            /// Initializes the texture reference if a new video source is detected. 
            /// </summary>
            /// <returns>True if a new video source is detected, false otherwise.</returns>
            private bool OnRenderEventInit()
            {
                if (m_CurrentVideoSource != VideoSource)
                {
                    UpdateCaptureType();
                    return true;
                }
                return false;
            }

            /// <summary>
            /// Initializes the texture reference if a new video source is detected. 
            /// </summary>
            /// <returns>True if a new video source is detected, false otherwise.</returns>
            private bool OnRenderEventUpdate()
            {
                if (m_CurrentVideoSource != VideoSource)
                {
                    UpdateCaptureType();
                    return true;
                }
                return false;
            }

            /// <summary>
            /// Retrieves the camera object from the source object.
            /// </summary>
            /// <param name="Source">The source can be a camera which contains a camera component.</param>
            private Camera GetCameraFromSource(UnityEngine.Object Source)
            {
                Camera camera = null;

                if (!(Source is Camera))
                {
                    var go = Source as GameObject;
                    if (go != null)
                    {
                        camera = go.GetComponent<Camera>();
                    }
                }
                else
                {
                    camera = (Camera)Source;
                }
                return camera;
            }

            /// <summary>
            /// Cleans up any added command-buffers from the source's camera.
            /// </summary>
            /// <param name="Source">The camera source.</param>
            private void CleanupCameraCommandBuffer(UnityEngine.Object Source)
            {
                if (Source != null)
                {
                    Camera camera = GetCameraFromSource(Source);

                    var listCommandBuffers = camera.GetCommandBuffers(CameraEvent.AfterEverything);
                    var findBuffer = false;

                    foreach (CommandBuffer x in listCommandBuffers)
                    {
                        if (x.name.Equals(m_CommandBufferName))
                        {
                            findBuffer = true;
                            break;
                        }
                    }

                    if (findBuffer)
                    {
                        camera.RemoveCommandBuffer(CameraEvent.AfterEverything, m_CommandBuffer);
                    }
                }
            }

            /// <summary>
            /// Initializes the texture reference using the current video source.
            /// Used for manual-capture mode.
            /// </summary>
            private void UpdateCaptureType()
            {
                var oldVideoSource = m_CurrentVideoSource;
                m_CurrentVideoSource = VideoSource;

                if (CaptureType != GenvidVideoParameters.eCaptureType.Automatic)
                {
                    if (GetCameraFromSource(oldVideoSource) != null)
                    {
                        CleanupCameraCommandBuffer(oldVideoSource);
                    }

                    if (CaptureType == GenvidVideoParameters.eCaptureType.Texture)
                    {
                        if (m_CurrentVideoSource is Texture)
                        {
                            m_TexturePtr = ((Texture)m_CurrentVideoSource).GetNativeTexturePtr();

                            var gvStatus = GenvidSDK.SetParameter(StreamName, "video.useopenglconvention", 1);
                            if (GenvidSDK.StatusFailed(gvStatus))
                            {
                                Debug.LogError("Error while setting the opengl convention: " + GenvidSDK.StatusToString(gvStatus));
                            }
                            else if (VerboseLog)
                            {
                                Debug.Log("Genvid Set Parameter useOpenGL performed correctly.");
                            }
                        }
                        else
                        {
                            var gvStatus = GenvidSDK.SetParameter(StreamName, "video.useopenglconvention", 0);
                            if (GenvidSDK.StatusFailed(gvStatus))
                            {
                                Debug.LogError("Error while setting the opengl convention: " + GenvidSDK.StatusToString(gvStatus));
                            }
                            else if (VerboseLog)
                            {
                                Debug.Log("Genvid Set Parameter useOpenGL performed correctly.");
                            }

                            Camera camera = GetCameraFromSource(m_CurrentVideoSource);
                            if (camera != null)
                            {
                                CaptureCamera(camera);
                            }
                            else
                            {
                                Debug.LogError("Cannot cast the current video source object: " + m_CurrentVideoSource.name);
                            }
                        }
                    }
                }
            }
#endif
        }
    }
}