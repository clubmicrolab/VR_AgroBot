using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoPilotControl : MonoBehaviour
{
    public float alphaThreshold = 0.1f;
    [SerializeField] private AutoPilot autoPilotScript; // Serialized field to reference the AutoPilot script
    private bool autoPilotActive = false;

    // Start is called before the first frame update
    void Start()
    {
        // No need for Image component; alphaHitTestMinimumThreshold not applicable without Image
    }

    // Update is called once per frame
    void Update()
    {
        // Check for key press (3 key)
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            autoPilotActive = !autoPilotActive;
            ActivateAutoPilot();
        }
    }

    void ActivateAutoPilot()
    {
        if (autoPilotScript != null)
        {
            // Modify the activation logic based on your requirements
            autoPilotScript.enabled = autoPilotActive;
        }
        else
        {
            Debug.LogWarning("AutoPilot script reference not set in the AutoPilotControl script.");
        }
    }
}
