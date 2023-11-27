using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeControl : MonoBehaviour
{
    public float alphaThreshold = 0.1f;
    [SerializeField] private ChargeStation chargeStationScript; // Serialized field to reference the ChargeStation script
    private bool chargeStationActive = false;

    // Start is called before the first frame update
    void Start()
    {
        // No need for Image component; alphaHitTestMinimumThreshold not applicable without Image
    }

    // Update is called once per frame
    void Update()
    {
        // Check for key press (4 key)
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            chargeStationActive = !chargeStationActive;
            ActivateChargeStation();
        }
    }

    void ActivateChargeStation()
    {
        if (chargeStationScript != null)
        {
            // Modify the activation logic based on your requirements
            chargeStationScript.enabled = chargeStationActive;
        }
        else
        {
            Debug.LogWarning("ChargeStation script reference not set in the script.");
        }
    }
}
