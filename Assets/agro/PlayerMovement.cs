using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform _carTransform;
    [SerializeField] private WheelCollider _wheel1;
    [SerializeField] private WheelCollider _wheel2;
    [SerializeField] private WheelCollider _wheel3;
    [SerializeField] private WheelCollider _wheel4;

    [SerializeField] private float _motorTorque = 10000f;
    [SerializeField] private float _maxAngle;
    [SerializeField] private float _brakeTorque = 20000f;

    private XRNode leftControllerNode = XRNode.LeftHand;
    private XRNode rightControllerNode = XRNode.RightHand;

    private bool _isMovingForward = true;

    private void FixedUpdate()
    {
        Move(); // Call the Move method to handle movement

        // Rotate left or right based on input from Oculus controllers
        float steeringInput = GetSteeringInput();
        ApplySteering(steeringInput);
    }

    private void Move()
    {
        // Check for gearbox button press to toggle movement direction
        if (IsGearboxButtonPressed())
        {
            _isMovingForward = !_isMovingForward; // Toggle movement direction
        }

        float verticalInput = GetVerticalInput(); // Get vertical input from Oculus controllers
        float horizontalInput = GetHorizontalInput(); // Get horizontal input from Oculus controllers

        float flSpeed, frSpeed, rlSpeed, rrSpeed;

        if (_isMovingForward)
        {
            flSpeed = verticalInput * _motorTorque; // Calculate front left wheel speed
            frSpeed = verticalInput * _motorTorque; // Calculate front right wheel speed
            rlSpeed = horizontalInput * _motorTorque; // Calculate rear left wheel speed
            rrSpeed = -horizontalInput * _motorTorque; // Calculate rear right wheel speed
        }
        else
        {
            flSpeed = -verticalInput * _motorTorque; // Calculate front left wheel speed for backward movement
            frSpeed = -verticalInput * _motorTorque; // Calculate front right wheel speed for backward movement
            rlSpeed = -horizontalInput * _motorTorque; // Calculate rear left wheel speed for backward movement
            rrSpeed = horizontalInput * _motorTorque; // Calculate rear right wheel speed for backward movement
        }

        _wheel1.motorTorque = flSpeed; // Apply motor torque to front left wheel
        _wheel2.motorTorque = frSpeed; // Apply motor torque to front right wheel
        _wheel3.motorTorque = rlSpeed; // Apply motor torque to rear left wheel
        _wheel4.motorTorque = rrSpeed; // Apply motor torque to rear right wheel

        // Apply brake torque to all wheels if a specific button on Oculus controllers is pressed
        if (IsBrakeButtonPressed())
        {
            ApplyBrakeTorque(_brakeTorque);
        }
        else
        {
            // Release brakes
            ApplyBrakeTorque(0f);
        }
    }

    private float GetVerticalInput()
    {
        // Get vertical input from Oculus controllers (e.g., thumbstick up/down or primaryHandTrigger)
        float verticalInput = 0f;
        float leftTriggerValue = GetTriggerInput(leftControllerNode);
        float rightTriggerValue = GetTriggerInput(rightControllerNode);

        // Use the trigger input to control forward or backward movement
        if (leftTriggerValue > 0 && rightTriggerValue > 0)
        {
            verticalInput = -1f; // Move backward when both triggers are pressed
        }
        else
        {
            InputDevice device = InputDevices.GetDeviceAtXRNode(leftControllerNode);
            device.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 thumbstickValue);
            verticalInput = thumbstickValue.y; // Use thumbstick input for forward movement
        }

        return verticalInput;
    }

    private float GetHorizontalInput()
    {
        // Get horizontal input from Oculus controllers (e.g., thumbstick left/right)
        float horizontalInput = 0f;
        InputDevice device = InputDevices.GetDeviceAtXRNode(rightControllerNode);
        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 thumbstickValue);

        // Use the horizontal component of the thumbstick value
        horizontalInput = thumbstickValue.x;

        return horizontalInput;
    }

    private bool IsBrakeButtonPressed()
    {
        // Check if a specific button on Oculus controllers (e.g., A or X) is pressed for braking
        InputDevice device = InputDevices.GetDeviceAtXRNode(leftControllerNode);
        bool isBrakeButtonPressed = false;
        device.TryGetFeatureValue(CommonUsages.secondaryButton, out isBrakeButtonPressed);
        return isBrakeButtonPressed;
    }

    private bool IsGearboxButtonPressed()
    {
        // Check if the B button on Oculus controllers is pressed for changing movement
        InputDevice device = InputDevices.GetDeviceAtXRNode(leftControllerNode);
        bool isGearboxButtonPressed = false;
        device.TryGetFeatureValue(CommonUsages.primaryButton, out isGearboxButtonPressed);
        return isGearboxButtonPressed;
    }

    private float GetTriggerInput(XRNode controllerNode)
    {
        float triggerValue = 0f;
        InputDevice device = InputDevices.GetDeviceAtXRNode(controllerNode);
        device.TryGetFeatureValue(CommonUsages.trigger, out triggerValue);
        return triggerValue;
    }

    private void ApplySteering(float steeringInput)
    {
        float maxSpeed = Mathf.Max(_wheel1.rpm, _wheel2.rpm, _wheel3.rpm, _wheel4.rpm);
        float maxSteeringSpeed = Mathf.Abs(_maxAngle * Mathf.Deg2Rad * maxSpeed);

        float flSteeringSpeed = maxSteeringSpeed * steeringInput;
        float frSteeringSpeed = maxSteeringSpeed * steeringInput;
        float rlSteeringSpeed = maxSteeringSpeed * steeringInput;
        float rrSteeringSpeed = maxSteeringSpeed * steeringInput;

        _wheel1.steerAngle = flSteeringSpeed;
        _wheel2.steerAngle = frSteeringSpeed;
        _wheel3.steerAngle = rlSteeringSpeed;
        _wheel4.steerAngle = rrSteeringSpeed;
    }

    private void ApplyBrakeTorque(float torque)
    {
        // Apply brake torque to all wheels
        _wheel1.brakeTorque = torque;
        _wheel2.brakeTorque = torque;
        _wheel3.brakeTorque = torque;
        _wheel4.brakeTorque = torque;
    }

    private float GetSteeringInput()
    {
        // Get steering input from Oculus controllers (e.g., thumbstick left/right)
        float steeringInput = 0f;
        InputDevice device = InputDevices.GetDeviceAtXRNode(rightControllerNode);
        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 thumbstickValue);

        // Use the horizontal component of the thumbstick value for steering
        steeringInput = thumbstickValue.x;

        return steeringInput;
    }
}
