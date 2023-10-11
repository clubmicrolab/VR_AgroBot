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

    private bool isMovingBackward = false;

    private void FixedUpdate()
    {
        Move(); // Call the Move method to handle movement

        // Rotate left or right based on input from Oculus controllers
        float steeringInput = GetSteeringInput();
        ApplySteering(steeringInput);

        // Toggle movement direction when B button is pressed
        if (IsBButtonPressed())
        {
            ToggleMovementDirection();
        }
    }

    private void Move()
    {
        float verticalInput = GetVerticalInput(); // Get vertical input from Oculus controllers
        float horizontalInput = GetHorizontalInput(); // Get horizontal input from Oculus controllers

        // Determine movement direction based on triggers
        if (verticalInput > 0.1f && horizontalInput > 0.1f)
        {
            // Both triggers pressed, enable forward movement
            verticalInput = 1f;
            isMovingBackward = false;
        }
        else if (verticalInput > 0.1f || horizontalInput > 0.1f)
        {
            // Either trigger pressed, enable backward movement
            verticalInput = -1f;
            isMovingBackward = true;
        }

        float flSpeed = verticalInput * _motorTorque; // Calculate front left wheel speed
        float frSpeed = verticalInput * _motorTorque; // Calculate front right wheel speed
        float rlSpeed = horizontalInput * _motorTorque; // Calculate rear left wheel speed
        float rrSpeed = -horizontalInput * _motorTorque; // Calculate rear right wheel speed

        if (isMovingBackward)
        {
            // Invert motor torque for backward movement
            flSpeed *= -1f;
            frSpeed *= -1f;
            rlSpeed *= -1f;
            rrSpeed *= -1f;
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
        // Get vertical input from Oculus controllers (e.g., triggers)
        float verticalInput = 0f;
        InputDevice device = InputDevices.GetDeviceAtXRNode(leftControllerNode);
        device.TryGetFeatureValue(CommonUsages.trigger, out verticalInput);
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

    private float GetSteeringInput()
    {
        float steeringInput = 0f;
        InputDevice device = InputDevices.GetDeviceAtXRNode(rightControllerNode);
        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 thumbstickValue);

        // Use the horizontal component of the thumbstick value for steering
        steeringInput = thumbstickValue.x;

        return steeringInput;
    }

    private bool IsBButtonPressed()
    {
        // Check if the B button on Oculus controllers is pressed
        InputDevice device = InputDevices.GetDeviceAtXRNode(rightControllerNode);
        bool isBButtonPressed = false;
        device.TryGetFeatureValue(CommonUsages.primaryButton, out isBButtonPressed);
        return isBButtonPressed;
    }

    private void ToggleMovementDirection()
    {
        isMovingBackward = !isMovingBackward;
    }

    private void ApplyBrakeTorque(float torque)
    {
        // Apply brake torque to all wheels
        _wheel1.brakeTorque = torque;
        _wheel2.brakeTorque = torque;
        _wheel3.brakeTorque = torque;
        _wheel4.brakeTorque = torque;
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
}
