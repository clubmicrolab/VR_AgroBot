using UnityEngine;

public class FullScreenMap : MonoBehaviour
{
    public GameObject miniMap;
    private bool isMiniMapVisible = true;

    void Start()
    {
        // Asociați mini-harta la începutul jocului
        miniMap.SetActive(isMiniMapVisible);
    }

    void Update()
    {
        // Afișați/ascundeți mini-harta când se apasă tasta "M"
        if (Input.GetKeyDown(KeyCode.M))
        {
            isMiniMapVisible = !isMiniMapVisible;
            miniMap.SetActive(isMiniMapVisible);
        }
    }
}
