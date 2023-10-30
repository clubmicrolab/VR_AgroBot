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
        SetAllObjectsActive(false); // Initially set all objects as inactive
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!allObjectsActive)
            {
                ToggleObjects(); // Toggle visibility of objects
            }
            else
            {
                SetAllObjectsActive(false); // Deactivate all objects
                currentIndex = 0;
                allObjectsActive = false;
            }
        }
    }

    private void ToggleObjects()
    {
        objectsToToggle[currentIndex].SetActive(true); // Activate the current object
        currentIndex = (currentIndex + 1) % objectsToToggle.Count; // Move to the next object
        allObjectsActive = currentIndex == 0; // Check if all objects are active
    }

    private void SetAllObjectsActive(bool active)
    {
        foreach (GameObject obj in objectsToToggle)
        {
            obj.SetActive(active);
        }
    }
}
