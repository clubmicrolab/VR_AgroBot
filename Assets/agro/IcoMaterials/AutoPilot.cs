using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AutoPilot : MonoBehaviour
{
    [SerializeField]
    private Transform enemy; // The enemy to move

    [SerializeField]
    private Transform target; // The target (player) for the enemy to follow

    private NavMeshAgent nav;

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();

        if (target == null)
        {
            // If the target is not set, find it by tag
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void Update()
    {
        if (enemy != null && target != null)
        {
            // Set the enemy's destination to the target (player) position
            nav.destination = target.position;
        }
    }
}
