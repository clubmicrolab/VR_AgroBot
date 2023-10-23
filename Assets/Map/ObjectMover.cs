using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    public Transform[] waypoints;
    private int waypointIndex = 0;
    public float moveSpeed = 2f;
    public float distanceThreshold = 0.1f;
    public float raycastDistance = 0.5f;  // Distance to check for obstacles

    private void Update()
    {
        if (waypoints == null || waypoints.Length == 0)
            return;

        Vector3 targetPosition = waypoints[waypointIndex].position;

        // Check for obstacles before moving
        if (!IsObstacleBetween(transform.position, targetPosition, raycastDistance))
        {
            // Move towards the target waypoint
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }

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

    // Check for obstacles between start and end points
    private bool IsObstacleBetween(Vector3 start, Vector3 end, float distance)
    {
        RaycastHit hit;
        if (Physics.Raycast(start, end - start, out hit, distance))
        {
            // If an obstacle is hit, return true
            return true;
        }
        return false;
    }
}
