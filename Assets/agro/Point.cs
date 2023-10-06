using UnityEngine;
using UnityEngine.UI;

public class Point : MonoBehaviour
{
    public Image img;
    public Transform target;
    public Text meter;
    public float maxDistance = 10f;  // Maximum distance for full size
    public float minDistance = 1f;   // Minimum distance for minimum size

    private void Update()
    {
        // Calculate the distance between the target and the player
        float distance = Vector3.Distance(target.position, transform.position);

        // Calculate the desired size based on the distance
        float desiredSize = Mathf.Lerp(img.rectTransform.sizeDelta.x, 100f, distance / maxDistance);
        img.rectTransform.sizeDelta = new Vector2(desiredSize, desiredSize);

        // Update the position as before
        float minX = img.GetPixelAdjustedRect().width / 2;
        float maxX = Screen.width - minX;
        float minY = img.GetPixelAdjustedRect().height / 2;
        float maxY = Screen.height - minY;

        Vector2 pos = Camera.main.WorldToScreenPoint(target.position);

        if (Vector3.Dot((target.position - transform.position), transform.forward) < 0)
        {
            if (pos.x < Screen.width / 2)
                pos.x = maxX;
            else
                pos.x = minX;
        }

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        img.transform.position = pos;
        meter.text = ((int)distance).ToString() + "m";
    }
}
