using UnityEngine;

public class ObjectDetector : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object has a name
        if (collision.gameObject.name != null)
        {
            // Print the name of the collided object to the console
            Debug.Log("Collided with: " + collision.gameObject.name);
        }
        else
        {
            // If the collided object doesn't have a name, print a generic message
            Debug.Log("Collided with an unnamed object");
        }
    }
}
