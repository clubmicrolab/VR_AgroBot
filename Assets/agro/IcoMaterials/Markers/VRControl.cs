using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRControl : MonoBehaviour
{
    public float alphaThreshold = 0.1f;
    [SerializeField] private PlayerMovement1 playerMovement1Script;
    [SerializeField] private Switch switchScript;

    private bool isPlayerMovement1Active = true; // Flag to track which movement script is currently active

    // Start is called before the first frame update
    void Start()
    {
        // No need for Image component; alphaHitTestMinimumThreshold not applicable without Image
    }

    // Update is called once per frame
    void Update()
    {
        // Check for key press (1 key)
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchMovementScript();
        }
    }

    void SwitchMovementScript()
    {
        if (isPlayerMovement1Active)
        {
            // Deactivate PlayerMovement1 and activate Switch
            if (playerMovement1Script != null)
            {
                playerMovement1Script.enabled = false;
            }

            if (switchScript != null)
            {
                switchScript.enabled = true;
            }

            isPlayerMovement1Active = false;
        }
        else
        {
            // Deactivate Switch and activate PlayerMovement1
            if (switchScript != null)
            {
                switchScript.enabled = false;
            }

            if (playerMovement1Script != null)
            {
                playerMovement1Script.enabled = true;
            }

            isPlayerMovement1Active = true;
        }
    }
}
