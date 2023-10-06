using UnityEngine;
public class MMtogglers : MonoBehaviour
{
    public GameObject fullScreenMap;
    private bool fullMap = false;
    void Update()
    {
        // Verificăm dacă tasta "M" a fost apăsată
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleFullScreenMap(); // Apelăm funcția pentru a deschide/închide harta
        }
    }
    public void ToggleFullScreenMap()
    {
        fullMap = !fullMap;
        fullScreenMap.SetActive(fullMap);
    }
}
