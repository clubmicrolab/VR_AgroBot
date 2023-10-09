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

    private bool isMoving; // Flag to track if the player is moving

    private void FixedUpdate()
    {
        // Detect Oculus Quest controller input
        float triggerValue = 0f;
        InputDevice device = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        device.TryGetFeatureValue(CommonUsages.trigger, out triggerValue);

        if (triggerValue > 0.1f)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        Move();

        if (Input.GetKey(KeyCode.A))
        {
            RotateLeft();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            RotateRight();
        }
    }

    private void Move()
    {
        float verticalInput = isMoving ? 1f : 0f;
        float horizontalInput = Input.GetAxis("Horizontal");

        float flSpeed = verticalInput * _motorTorque;
        float frSpeed = verticalInput * _motorTorque;
        float rlSpeed = horizontalInput * _motorTorque;
        float rrSpeed = -horizontalInput * _motorTorque;

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

    private void RotateLeft()
    {
        _carTransform.Rotate(Vector3.up, -_maxAngle * Time.deltaTime);
    }

    private void RotateRight()
    {
        _carTransform.Rotate(Vector3.up, _maxAngle * Time.deltaTime);
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