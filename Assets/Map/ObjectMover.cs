using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    public Transform[] waypoints;
    private int waypointIndex = 0;
    public float moveSpeed = 2f;
    public float distanceThreshold = 0.1f;

    private void Update()
    {
        if (waypoints == null || waypoints.Length == 0)
            return;

        Vector3 targetPosition = waypoints[waypointIndex].position;

        // Move towards the target waypoint
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Check if the distance to the waypoint is less than the threshold
        if (Vector3.Distance(transform.position, targetPosition) < distanceThreshold)
        {
            waypointIndex++;

            // If the waypoint index is greater than or equal to the number of waypoints, reset it to 0.
            if (waypointIndex >= waypoints.Length)
            {
                waypointIndex = 0;
            }
        }
    }
}
