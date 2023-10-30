using UnityEngine;
using System.Collections.Generic;

public class Placer : MonoBehaviour
{
    [SerializeField] Camera selectedCamera;
    [SerializeField] Vector3 offsetValue;
    [SerializeField] List<GameObject> objectsToTeleport;

    private bool isFollowingMouse = false;
    private int currentObjectIndex = 0;
    private bool allObjectsPlaced = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (allObjectsPlaced)
            {
                // All objects are placed, hide them and reset for the next cycle
                HideAllObjects();
                currentObjectIndex = 0;
                allObjectsPlaced = false;
            }

            if (isFollowingMouse)
            {
                isFollowingMouse = false;
                currentObjectIndex = (currentObjectIndex + 1) % objectsToTeleport.Count; // Move to the next object in the list
            }
            else
            {
                Vector3 mousePosition = Input.mousePosition;
                mousePosition.z = Mathf.Abs(selectedCamera.transform.position.z);

                Vector3 clickPosition = selectedCamera.ScreenToWorldPoint(mousePosition);

                Debug.Log($"Mouse point: {mousePosition}\r Click Point: {clickPosition}");

                Teleport(clickPosition, objectsToTeleport[currentObjectIndex]);
                isFollowingMouse = true;
            }
        }

        if (isFollowingMouse)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Mathf.Abs(selectedCamera.transform.position.z);
            Vector3 newPosition = selectedCamera.ScreenToWorldPoint(mousePosition);

            newPosition -= offsetValue;
            objectsToTeleport[currentObjectIndex].transform.position = newPosition;
        }
    }

    void Teleport(Vector3 tapPosition, GameObject objectToTeleport)
    {
        Vector3 newPosition = new Vector3(tapPosition.x - offsetValue.x, objectToTeleport.transform.position.y, tapPosition.z - offsetValue.z);
        objectToTeleport.transform.position = newPosition;

        objectToTeleport.SetActive(true); // Set the object active

        // Check if this is the last object
        if (currentObjectIndex == objectsToTeleport.Count - 1)
        {
            allObjectsPlaced = true; // All objects are placed
        }
    }

    void HideAllObjects()
    {
        foreach (var obj in objectsToTeleport)
        {
            obj.SetActive(false); // Hide all objects
        }
    }
}
