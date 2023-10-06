using UnityEngine;

public class MiniMapController : MonoBehaviour
{
    public GameObject miniMap;

    void Start()
    {
        // Asociați mini-harta la începutul jocului
        miniMap.SetActive(true); // Vă asigurați că mini-harta este activată la început
    }
}
