using UnityEngine;
using UnityEngine.UI;

public class Clickable2 : MonoBehaviour
{
    public float alphaThreshold = 0.1f;
    [SerializeField] private AutoPilot autoPilotScript; // Serialized field to reference the AutoPilot script
    [SerializeField] private ChargeStation chargeStationScript; // Serialized field to reference the ChargeStation script

    // Start is called before the first frame update
    void Start()
    {
        Image image = GetComponent<Image>();
        image.alphaHitTestMinimumThreshold = alphaThreshold;
    }

    // Update is called once per frame
    void Update()
    {
        // Check for key press (3 key)
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ActivateAutoPilot();
        }
    }

    void ActivateAutoPilot()
    {
        if (autoPilotScript != null)
        {
            autoPilotScript.enabled = true;

            // Deactivate ChargeStation script if it's not null
            if (chargeStationScript != null)
            {
                chargeStationScript.enabled = false;
            }
        }
        else
        {
            Debug.LogWarning("AutoPilot script reference not set in the Clickable2 script.");
        }
    }
}
