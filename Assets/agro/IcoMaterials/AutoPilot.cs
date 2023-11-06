using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AutoPilot : MonoBehaviour
{
    [SerializeField]
    private Transform Agrobot;

    [SerializeField]
    private List<Transform> markers;
    private NavMeshAgent nav;
    private int currentMarkerIndex = 0; 

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();

        if (markers.Count == 0)
        {
            GameObject[] markerObjects = GameObject.FindGameObjectsWithTag("Marker");
            foreach (GameObject markerObject in markerObjects)
            {
                markers.Add(markerObject.transform);
            }
        }

        SetNextDestination(); 
    }

    void Update()
    {
        if (Agrobot != null && markers.Count > 0)
        {
            if (!nav.pathPending && nav.remainingDistance < 0.1f)
            {
                SetNextDestination();
            }
        }
    }

    void SetNextDestination()
    {
        if (markers.Count > 0)
        {
            nav.destination = markers[currentMarkerIndex].position;
            currentMarkerIndex = (currentMarkerIndex + 1) % markers.Count;
        }
    }
}
