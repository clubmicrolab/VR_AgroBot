using System.Collections;
using UnityEngine;

public class Charge : MonoBehaviour
{
    [SerializeField]
    private Transform modelulInMiscare;

    [SerializeField]
    private Transform obiectulTinta;

    [SerializeField]
    private Material[] materialeHealthBar;

    [SerializeField]
    private Renderer rendererHealthBar;

    private int indexMaterialCurent = 0;
    private int indexMaterialMaxim;
    private bool atingereObiectTinta = false;

    private float timpSchimbareMaterial = 20.0f;
    private float timpCurentSchimbare = 0.0f;
    private bool schimbareInProgres = false;

    void Start()
    {
        indexMaterialMaxim = materialeHealthBar.Length - 1;
        if (rendererHealthBar != null)
        {
            rendererHealthBar.material = materialeHealthBar[indexMaterialCurent];
        }
    }

    void Update()
    {
        if (!schimbareInProgres)
        {
            if (modelulInMiscare != null && modelulInMiscare.position != transform.position)
            {
                schimbareInProgres = true;
                StartCoroutine(SchimbaMaterialInMiscare());
            }

            if (obiectulTinta != null && atingereObiectTinta)
            {
                schimbareInProgres = true;
                StartCoroutine(SchimbaMaterialObiectTinta());
            }
        }
    }

    IEnumerator SchimbaMaterialInMiscare()
    {
        int materialAnterior = indexMaterialCurent;
        while (timpCurentSchimbare < timpSchimbareMaterial)
        {
            timpCurentSchimbare += Time.deltaTime;
            indexMaterialCurent = (int)Mathf.Lerp(materialAnterior, indexMaterialMaxim, timpCurentSchimbare / timpSchimbareMaterial);
            rendererHealthBar.material = materialeHealthBar[indexMaterialCurent];
            yield return null;
        }
        timpCurentSchimbare = 0.0f;
        schimbareInProgres = false;
    }

    IEnumerator SchimbaMaterialObiectTinta()
    {
        int materialAnterior = indexMaterialCurent;
        while (timpCurentSchimbare < timpSchimbareMaterial)
        {
            timpCurentSchimbare += Time.deltaTime;
            indexMaterialCurent = (int)Mathf.Lerp(materialAnterior, 0, timpCurentSchimbare / timpSchimbareMaterial);
            rendererHealthBar.material = materialeHealthBar[indexMaterialCurent];
            yield return null;
        }
        indexMaterialCurent = 0;
        rendererHealthBar.material = materialeHealthBar[indexMaterialCurent];
        timpCurentSchimbare = 0.0f;
        schimbareInProgres = false;
        atingereObiectTinta = false;
    }

    public void SetModelInMiscare(Transform model)
    {
        modelulInMiscare = model;
    }

    public void SetObiectTinta(Transform obiect)
    {
        obiectulTinta = obiect;
        atingereObiectTinta = true;
    }
}
