using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestrictYAxis : MonoBehaviour
{
    private float fixedYPosition = 28.55465f;

    private void Update()
    {
        // Ensure the Y-axis position stays constant
        Vector3 newPosition = transform.position;
        newPosition.y = fixedYPosition;
        transform.position = newPosition;
    }
}
