using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform _carTransform; // Reference to the car's main transform
    [SerializeField] private WheelCollider _wheel1;
    [SerializeField] private WheelCollider _wheel2;
    [SerializeField] private WheelCollider _wheel3; // Rear left wheel collider
    [SerializeField] private WheelCollider _wheel4; // Rear right wheel collider

    [SerializeField] private float _motorTorque = 10000f; // Increased motor torque for faster movement
    [SerializeField] private float _maxAngle;
    [SerializeField] private float _brakeTorque = 20000f; // Brake torque for stopping all wheels

    private void FixedUpdate()
    {
        Move(); // Call the Move method to handle movement

        // Rotate left or right based on A and D keys
        if (Input.GetKey(KeyCode.A))
        {
            RotateLeft(); // Rotate left if A key is pressed
        }
        else if (Input.GetKey(KeyCode.D))
        {
            RotateRight(); // Rotate right if D key is pressed
        }
    }

    private void Move()
    {
        float verticalInput = Input.GetAxis("Vertical"); // Get vertical input (W or S keys)
        float horizontalInput = Input.GetAxis("Horizontal"); // Get horizontal input (A or D keys)

        float flSpeed = verticalInput * _motorTorque; // Calculate front left wheel speed
        float frSpeed = verticalInput * _motorTorque; // Calculate front right wheel speed
        float rlSpeed = horizontalInput * _motorTorque; // Calculate rear left wheel speed
        float rrSpeed = -horizontalInput * _motorTorque; // Calculate rear right wheel speed

        _wheel1.motorTorque = flSpeed; // Apply motor torque to front left wheel
        _wheel2.motorTorque = frSpeed; // Apply motor torque to front right wheel
        _wheel3.motorTorque = rlSpeed; // Apply motor torque to rear left wheel
        _wheel4.motorTorque = rrSpeed; // Apply motor torque to rear right wheel

        // Apply brake torque to all wheels if F key is pressed
        if (Input.GetKey(KeyCode.F))
        {
            ApplyBrakeTorque(_brakeTorque);
        }
        else
        {
            // Release brakes
            ApplyBrakeTorque(0f);
        }

        float steeringInput = Input.GetAxis("Horizontal"); // Get steering input (A or D keys)
        ApplySteering(steeringInput); // Apply steering based on input
    }

    private void ApplyBrakeTorque(float torque)
    {
        // Apply brake torque to all wheels
        _wheel1.brakeTorque = torque;
        _wheel2.brakeTorque = torque;
        _wheel3.brakeTorque = torque;
        _wheel4.brakeTorque = torque;
    }

    private void RotateLeft()
    {
        // Rotate left logic here
        _carTransform.Rotate(Vector3.up, -_maxAngle * Time.deltaTime); // Rotate the car to the left
    }

    private void RotateRight()
    {
        // Rotate right logic here
        _carTransform.Rotate(Vector3.up, _maxAngle * Time.deltaTime); // Rotate the car to the right
    }

    private void ApplySteering(float steeringInput)
    {
        float maxSpeed = Mathf.Max(_wheel1.rpm, _wheel2.rpm, _wheel3.rpm, _wheel4.rpm); // Calculate maximum wheel speed
        float maxSteeringSpeed = Mathf.Abs(_maxAngle * Mathf.Deg2Rad * maxSpeed); // Calculate maximum steering speed

        float flSteeringSpeed = maxSteeringSpeed * steeringInput; // Calculate front left steering speed
        float frSteeringSpeed = maxSteeringSpeed * steeringInput; // Calculate front right steering speed
        float rlSteeringSpeed = maxSteeringSpeed * steeringInput; // Calculate rear left steering speed
        float rrSteeringSpeed = maxSteeringSpeed * steeringInput; // Calculate rear right steering speed

        _wheel1.steerAngle = flSteeringSpeed; // Apply steering to front left wheel
        _wheel2.steerAngle = frSteeringSpeed; // Apply steering to front right wheel
        _wheel3.steerAngle = rlSteeringSpeed; // Apply steering to rear left wheel
        _wheel4.steerAngle = rrSteeringSpeed; // Apply steering to rear right wheel
    }
}
