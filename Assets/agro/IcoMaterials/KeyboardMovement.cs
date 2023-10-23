using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardMovement : MonoBehaviour
{
    [SerializeField] private Transform _carTransform; // Reference to the car's main transform
    [SerializeField] private WheelCollider _colliderFL;
    [SerializeField] private WheelCollider _colliderFR;
    [SerializeField] private WheelCollider _colliderRL; // Rear left wheel collider
    [SerializeField] private WheelCollider _colliderRR; // Rear right wheel collider

    [SerializeField] private float _force = 10000f; // Increased force for faster movement
    [SerializeField] private float _maxAngle;
    [SerializeField] private float _brakeTorque = 20000f; // Brake torque for stopping all wheels

    private void FixedUpdate()
    {
        Move();

        // Rotate left or right based on A and D keys
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
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        float flSpeed = verticalInput * _force;
        float frSpeed = verticalInput * _force;
        float rlSpeed = horizontalInput * _force;
        float rrSpeed = -horizontalInput * _force;

        _colliderFL.motorTorque = flSpeed;
        _colliderFR.motorTorque = frSpeed;
        _colliderRL.motorTorque = rlSpeed;
        _colliderRR.motorTorque = rrSpeed;

        // Apply brake torque to all wheels
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyBrakeTorque(_brakeTorque);
        }
        else
        {
            // Release brakes
            ApplyBrakeTorque(0f);
        }

        float steeringInput = Input.GetAxis("Horizontal");
        ApplySteering(steeringInput);
    }

    private void ApplyBrakeTorque(float torque)
    {
        // Apply brake torque to all wheels
        _colliderFL.brakeTorque = torque;
        _colliderFR.brakeTorque = torque;
        _colliderRL.brakeTorque = torque;
        _colliderRR.brakeTorque = torque;
    }

    private void RotateLeft()
    {
        // Rotate left logic here
        _carTransform.Rotate(Vector3.up, -_maxAngle * Time.deltaTime);
    }

    private void RotateRight()
    {
        // Rotate right logic here
        _carTransform.Rotate(Vector3.up, _maxAngle * Time.deltaTime);
    }

    private void ApplySteering(float steeringInput)
    {
        float maxSpeed = Mathf.Max(_colliderFL.rpm, _colliderFR.rpm, _colliderRL.rpm, _colliderRR.rpm);
        float maxSteeringSpeed = Mathf.Abs(_maxAngle * Mathf.Deg2Rad * maxSpeed);

        float flSteeringSpeed = maxSteeringSpeed * steeringInput;
        float frSteeringSpeed = maxSteeringSpeed * steeringInput;
        float rlSteeringSpeed = maxSteeringSpeed * steeringInput;
        float rrSteeringSpeed = maxSteeringSpeed * steeringInput;

        _colliderFL.steerAngle = flSteeringSpeed;
        _colliderFR.steerAngle = frSteeringSpeed;
        _colliderRL.steerAngle = rlSteeringSpeed;
        _colliderRR.steerAngle = rrSteeringSpeed;
    }
}

