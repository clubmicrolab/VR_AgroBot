using UnityEngine;
using UnityEngine.XR;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform _carTransform;
    [SerializeField] private WheelCollider _wheel1;
    [SerializeField] private WheelCollider _wheel2;
    [SerializeField] private WheelCollider _wheel3;
    [SerializeField] private WheelCollider _wheel4;

    [SerializeField] private float _motorTorque = 10000f; // Corrected variable name
    [SerializeField] private float _maxAngle;
    [SerializeField] private float _brakeTorque = 20000f;

    private void FixedUpdate()
    {
        // Detect Oculus Quest controller input
        float leftTriggerValue = 0f;
        float rightTriggerValue = 0f;

        InputDevice leftDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        InputDevice rightDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        leftDevice.TryGetFeatureValue(CommonUsages.trigger, out leftTriggerValue);
        rightDevice.TryGetFeatureValue(CommonUsages.trigger, out rightTriggerValue);

        bool isMoving = leftTriggerValue > 0.1f && rightTriggerValue > 0.1f;
        bool isRotatingLeft = leftTriggerValue > 0.1f && rightTriggerValue < 0.1f;
        bool isRotatingRight = rightTriggerValue > 0.1f && leftTriggerValue < 0.1f;

        Move(leftTriggerValue, rightTriggerValue, isMoving);

        if (isRotatingLeft)
        {
            RotateLeft();
        }
        else if (isRotatingRight)
        {
            RotateRight();
        }
    }

    private void Move(float leftTriggerValue, float rightTriggerValue, bool isMoving)
    {
        float triggerAverage = (leftTriggerValue + rightTriggerValue) / 2;  // Calculate the average trigger value

        float verticalInput = isMoving ? 1f : 0f;

        // Adjust motor torque based on trigger sensitivity
        float torqueModifier = Mathf.Lerp(0.2f, 1f, triggerAverage);  // Modify these values to adjust sensitivity
        float motorTorque = _motorTorque * torqueModifier; // Corrected variable name

        float flSpeed = verticalInput * motorTorque;
        float frSpeed = verticalInput * motorTorque;
        float rlSpeed = verticalInput * motorTorque;
        float rrSpeed = verticalInput * motorTorque;

        _wheel1.motorTorque = flSpeed;
        _wheel2.motorTorque = frSpeed;
        _wheel3.motorTorque = rlSpeed;
        _wheel4.motorTorque = rrSpeed;

        if (Input.GetKey(KeyCode.F))
        {
            ApplyBrakeTorque(_brakeTorque);
        }
        else
        {
            ApplyBrakeTorque(0f);
        }

        float steeringInput = Input.GetAxis("Horizontal");
        ApplySteering(steeringInput);
    }

    private void ApplyBrakeTorque(float torque)
    {
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

    private void RotateLeft()
    {
        _carTransform.Rotate(Vector3.up, -_maxAngle * Time.deltaTime);
    }

    private void RotateRight()
    {
        _carTransform.Rotate(Vector3.up, _maxAngle * Time.deltaTime);
    }
}
