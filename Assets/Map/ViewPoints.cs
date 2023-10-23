using UnityEngine;

public class ViewPoints : MonoBehaviour
{
    public float speed = 1.0f;  // Speed of the platform's movement
    public float distance = 2.0f;  // Distance the platform will move up and down

    private float initialY;  // Initial Y position of the platform

    void Start()
    {
        initialY = transform.position.y;  // Store the initial Y position
    }

    void Update()
    {
        // Calculate the new Y position based on sine wave for smooth up and down movement
        float newY = initialY + Mathf.Sin(Time.time * speed) * distance;

        // Update the platform's position
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
