using UnityEngine;
using System.Collections.Generic;

public class CameraController : MonoBehaviour
{
    public List<Transform> obiecteStatice; // Lista de obiecte care nu se vor mișca cu camera

    void Update()
    {
        // Actualizăm pozițiile obiectelor statice pentru a le menține fixe în cadrul camerei
        foreach (Transform obiectStatic in obiecteStatice)
        {
            if (obiectStatic != null)
            {
                // Obținem poziția inițială a obiectului și o adaptăm la noua poziție a camerei
                Vector3 offset = transform.position - Camera.main.transform.position;
                Vector3 pozitieNoua = obiectStatic.position + offset;
                obiectStatic.position = pozitieNoua;
            }
        }
    }
}

