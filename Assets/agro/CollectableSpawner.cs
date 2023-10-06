using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class CollectableSpawner : MonoBehaviour
{
    [SerializeField]
    private Collectable prefab;
    [SerializeField]
    private Transform player;
    [SerializeField]
    private LineRenderer path;
    [SerializeField]
    private float pathHeightOffset = 1.25f;
    [SerializeField]
    private float spawnHeightOffset = 1.5f;
    [SerializeField]
    private float pathUpdateSpeed = 0.25f;

    private Collectable activeInstance;
    private NavMeshTriangulation triangulation;
    private Coroutine drawPathCoroutine;

    private void Awake()
    {
        triangulation = NavMesh.CalculateTriangulation();
    }

    private void Start()
    {
        SpawnNewObject();
        if (path.material == null)
        {
            Material newMaterial = new Material(Shader.Find("Sprites/Default"));
            path.material = newMaterial;
        }
        if (!path.gameObject.activeSelf)
        {
            path.gameObject.SetActive(true);
        }
        if (path.material != null)
        {
            path.material.color = Color.white;
            path.material.SetFloat("_Mode", 2);
            path.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            path.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            path.material.SetInt("_ZWrite", 0);
            path.material.DisableKeyword("_ALPHATEST_ON");
            path.material.EnableKeyword("_ALPHABLEND_ON");
            path.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            path.material.renderQueue = 3000;
        }
    }

    public void SpawnNewObject()
    {
        if (triangulation.vertices == null || triangulation.vertices.Length == 0)
        {
            Debug.LogWarning("Nu există vârfuri disponibile pentru spawn.");
            return;
        }
        int randomIndex = Random.Range(0, triangulation.vertices.Length);
        Vector3 spawnPosition = triangulation.vertices[randomIndex] + Vector3.up * spawnHeightOffset;
        activeInstance = Instantiate(prefab, spawnPosition, Quaternion.Euler(90, 0, 0));
        activeInstance.spawner = this;

        if (drawPathCoroutine != null)
        {
            StopCoroutine(drawPathCoroutine);
        }
        drawPathCoroutine = StartCoroutine(DrawPathCollectable());
    }

    private IEnumerator DrawPathCollectable()
    {
        WaitForSeconds wait = new WaitForSeconds(pathUpdateSpeed);
        NavMeshPath pathNav = new NavMeshPath();

        while (activeInstance != null)
        {
            if (NavMesh.CalculatePath(player.position, activeInstance.transform.position, NavMesh.AllAreas, pathNav))
            {
                path.positionCount = pathNav.corners.Length;

                for (int i = 0; i < pathNav.corners.Length; i++)
                {
                    path.SetPosition(i, pathNav.corners[i] + Vector3.up * pathHeightOffset);
                }
            }
            else
            {
                Debug.LogError($"Imposibil de calculat o cale pe NavMesh între {player.position} și {activeInstance.transform.position}!");
            }

            yield return wait;
        }
    }
}
