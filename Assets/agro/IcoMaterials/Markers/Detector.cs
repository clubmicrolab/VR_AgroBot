using UnityEngine;

public class Detector : MonoBehaviour
{
    public GameObject model; // Modelul pe care-l detectăm
    public GameObject detectorObiect; // Obiectul care detectează apropierea

    [SerializeField] private MonoBehaviour healthBarControlScript; // Referință la scriptul HealthBar
    [SerializeField] private MonoBehaviour chargeScript; // Referință la scriptul Charge

    public float distantaSchimbare = 5f; // Distanța la care se face schimbarea între scripturi

    private bool modelAproape = false;

    void Start()
    {
        healthBarControlScript = GameObject.Find("HealthBar").GetComponent<MonoBehaviour>();
        chargeScript = GameObject.Find("StationMark").GetComponent<MonoBehaviour>();

        // Inițializăm starea scripturilor la pornit/oprit
        healthBarControlScript.enabled = true;
        chargeScript.enabled = false;
    }

    void Update()
    {
        float distanta = Vector3.Distance(detectorObiect.transform.position, model.transform.position);

        if (distanta < distantaSchimbare && !modelAproape)
        {
            modelAproape = true;

            // Mai întâi activăm scriptul de încărcare și apoi scriptul de control al barei de sănătate
            chargeScript.enabled = true;
            healthBarControlScript.enabled = false;
        }
        else if (distanta >= distantaSchimbare && modelAproape)
        {
            modelAproape = false;

            // La depășirea distanței, dezactivăm scriptul de încărcare și activăm scriptul de control al barei de sănătate
            chargeScript.enabled = false;
            healthBarControlScript.enabled = true;
        }
    }
}
