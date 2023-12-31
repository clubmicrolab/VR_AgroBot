﻿using UnityEngine;
using UnityEngine.XR;

public class Switch : MonoBehaviour
{
    public PlayerMovement1 PlayerMovement1; // Initial active script
    public PlayerMovement2 PlayerMovement2; // Initial inactive script
    public XRNode rightControllerNode = XRNode.RightHand; // Adjust as needed

    private bool isPlayerMovement1Active = true; // Flag to track active script

    void Start()
    {
        // Ensure the initial states match the desired setup
        PlayerMovement1.enabled = true;
        PlayerMovement2.enabled = false;
    }

    void Update()
    {
        // Check if the "Switch" button (Button B) is pressed on the Oculus Quest 2 controller
        if (IsSwitchButtonPressed())
        {
            // Toggle the active state of the scripts
            isPlayerMovement1Active = !isPlayerMovement1Active;

            PlayerMovement1.enabled = isPlayerMovement1Active;
            PlayerMovement2.enabled = !isPlayerMovement1Active;
        }
    }

    private bool IsSwitchButtonPressed()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(rightControllerNode);
        bool isSwitchButtonPressed = false;
        device.TryGetFeatureValue(CommonUsages.secondaryButton, out isSwitchButtonPressed);
        return isSwitchButtonPressed;
    }
}
