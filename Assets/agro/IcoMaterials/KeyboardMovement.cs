using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardMovement : MonoBehaviour
{
    [SerializeField] private Transform _carTransform;
    [SerializeField] private WheelCollider _colliderFL;
    [SerializeField] private WheelCollider _colliderFR;
    [SerializeField] private WheelCollider _colliderRL;
    [SerializeField] private WheelCollider _colliderRR;

    [SerializeField] private float _force = 10000f;
    [SerializeField] private float _maxAngle;
    [SerializeField] private float _brakeTorque = 20000f;

    private void FixedUpdate()
    {
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
        bool isAccelerating = Input.GetKey(KeyCode.W);

        float horizontalInput = Input.GetAxis("Horizontal");
        float speed = isAccelerating ? _force : 0f;

        float steering = horizontalInput * _maxAngle;

        _colliderFL.motorTorque = speed;
        _colliderFR.motorTorque = speed;
        _colliderRL.motorTorque = -steering;
        _colliderRR.motorTorque = steering;

        if (Input.GetKey(KeyCode.Space))
        {
            ApplyBrakeTorque(_brakeTorque);
        }
        else
        {
            // Eliberează frânele
            ApplyBrakeTorque(0f);
        }

        ApplySteering(horizontalInput);
    }

    private void ApplyBrakeTorque(float torque)
    {
        _colliderFL.brakeTorque = torque;
        _colliderFR.brakeTorque = torque;
        _colliderRL.brakeTorque = torque;
        _colliderRR.brakeTorque = torque;
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
        float maxSteerAngle = 30f;
        float steerAngle = maxSteerAngle * steeringInput;

        _colliderFL.steerAngle = steerAngle;
        _colliderFR.steerAngle = steerAngle;
        _colliderRL.steerAngle = steerAngle;
        _colliderRR.steerAngle = steerAngle;
    }
}
