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
        // Check for left mouse button click
        if (Input.GetMouseButtonDown(0))
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
