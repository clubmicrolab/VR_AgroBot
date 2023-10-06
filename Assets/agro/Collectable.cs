using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField]
    private Vector3 spinAmount = new Vector3(0, 20, 0);
    public CollectableSpawner spawner;

    void Update()
    {
        transform.rotation *= Quaternion.Euler(spinAmount * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
        spawner.SpawnNewObject();
    }
}
