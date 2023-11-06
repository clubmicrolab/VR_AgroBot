using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vrCamera : MonoBehaviour
{
    [SerializeField] private Transform _car;

    private Vector3 _offset = new Vector3(0f, 8f, -35f);
    private float _speed = 10f;

    private void FixedUpdate()
    {
        // Calculate the target position relative to the car
        var targetPosition = _car.TransformPoint(_offset);

        // Move the camera towards the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, _speed * Time.deltaTime);

        // Look at the car
        transform.LookAt(_car.position);
    }
}
