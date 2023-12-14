using UnityEngine;

public class PositionCatcher : MonoBehaviour
{
    [SerializeField]
    private Transform agrobotTransform;

    private Vector3 previousPosition;
    private Vector3 speed;
    private Vector3 previousSpeed;
    private Vector3 acceleration;

    [SerializeField]
    private float thresholdSpeed = 5.0f;

    void Start()
    {
        if (agrobotTransform == null)
        {
            agrobotTransform = GetComponent<Transform>();
        }

        previousPosition = agrobotTransform.position;
        previousSpeed = Vector3.zero;
    }

    void Update()
    {
        Vector3 agrobotPosition = agrobotTransform.position;
        speed = (agrobotPosition - previousPosition) / Time.deltaTime;

        // Calculate acceleration based on the difference in speed over time
        acceleration = (speed - previousSpeed) / Time.deltaTime;

        previousPosition = agrobotPosition;
        previousSpeed = speed;

        Debug.Log($"Agrobot Position: {agrobotPosition}");
        Debug.Log($"Speed: {speed} m/s");
        Debug.Log($"Acceleration: {acceleration} m/s²");

        if (speed.magnitude > thresholdSpeed)
        {
            Debug.Log("Agrobot is moving faster!");
        }

        System.DateTime currentTime = System.DateTime.Now;
        Debug.Log($"Current Time: {currentTime.ToString("HH:mm:ss")}");
    }
}
