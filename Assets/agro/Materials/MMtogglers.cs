using UnityEngine;

public class MMtogglers : MonoBehaviour
{
    public Camera fullScreenCamera;
    private bool cameraActiv = true;

    void Update()
    {
        // Verificăm dacă tasta "M" a fost apăsată
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleCameraActivation();
        }
    }

    public void ToggleCameraActivation()
    {
        cameraActiv = !cameraActiv;
        fullScreenCamera.enabled = cameraActiv;
    }
}
