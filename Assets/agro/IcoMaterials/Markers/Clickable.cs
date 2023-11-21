using UnityEngine;
using UnityEngine.UI;

public class Clickable : MonoBehaviour
{
    public float alphaThreshold = 0.1f;
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
        // Check for key press (1 key)
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ActivateChargeStation();
        }
    }

    void ActivateChargeStation()
    {
        if (chargeStationScript != null)
        {
            chargeStationScript.enabled = true;
        }
        else
        {
            Debug.LogWarning("ChargeStation script reference not set in the Clickable script.");
        }
    }
}
