using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Markers : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> objectsToToggle = new List<GameObject>();

    private int currentIndex = 0;
    private bool allObjectsActive = false;

    private void Start()
    {
        // Asigură-te că inițial toate obiectele sunt inactive (ascunse).
        foreach (GameObject obj in objectsToToggle)
        {
            obj.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!allObjectsActive)
            {
                // Activează următorul obiect în listă și dezactivează obiectul curent
                objectsToToggle[currentIndex].SetActive(false);
                currentIndex = (currentIndex + 1) % objectsToToggle.Count;
                objectsToToggle[currentIndex].SetActive(true);

                // Verifică dacă toate obiectele sunt active
                allObjectsActive = currentIndex == 0;
            }
            else
            {
                // Resetarea procesului: dezactivează toate obiectele și activează primul
                foreach (GameObject obj in objectsToToggle)
                {
                    obj.SetActive(false);
                }
                currentIndex = 0;
                objectsToToggle[currentIndex].SetActive(true);
                allObjectsActive = false;
            }
        }
    }
}
