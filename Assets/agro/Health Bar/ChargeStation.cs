using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))] // Add this line to require a Rigidbody component
public class ChargeStation : MonoBehaviour
{
    [SerializeField] private Transform Agrobot;
    [SerializeField] private List<Transform> markers;
    private NavMeshAgent nav;
    private Rigidbody rb; // Reference to the Rigidbody component
    private int currentMarkerIndex = 0;
    private bool hasReachedDestination = false;
    private bool isWaiting = false;
    private float waitTime = 5.0f;

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component

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
            if (!isWaiting)
            {
                if (!hasReachedDestination && !nav.pathPending && nav.remainingDistance < 0.1f)
                {
                    hasReachedDestination = true;
                    StartCoroutine(WaitAtDestination());
                }
            }
        }
    }

    IEnumerator WaitAtDestination()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        isWaiting = false;
        hasReachedDestination = false;
        SetNextDestination();
    }

    void SetNextDestination()
    {
        if (markers.Count > 0)
        {
            nav.destination = markers[currentMarkerIndex].position;
            currentMarkerIndex = (currentMarkerIndex + 1) % markers.Count;
        }
    }

    public void SetWaitTime(float newWaitTime)
    {
        waitTime = newWaitTime;
    }
}
