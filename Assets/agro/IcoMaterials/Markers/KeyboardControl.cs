using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardControl : MonoBehaviour
{
    public float alphaThreshold = 0.1f;
    [SerializeField] private KeyboardMovement keyboardMovementScript; // Serialized field to reference the KeyboardMovement script

    // Start is called before the first frame update
    void Start()
    {
        // No need for Image component; alphaHitTestMinimumThreshold not applicable without Image
    }

    // Update is called once per frame
    void Update()
    {
        // Check for key press (1 key)
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ActivateKeyboardMovement();
        }
    }

    void ActivateKeyboardMovement()
    {
        if (keyboardMovementScript != null)
        {
            // Modify the activation logic based on your requirements
            keyboardMovementScript.enabled = !keyboardMovementScript.enabled;
        }
        else
        {
            Debug.LogWarning("KeyboardMovement script reference not set in the Clickable script.");
        }
    }
}
