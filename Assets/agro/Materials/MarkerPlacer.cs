using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class MarkerPlacer : MonoBehaviour, IPointerClickHandler
{
    public Image imaginePrefab; // Prefab-ul imaginii pentru pin
    public TextMeshProUGUI textMeshProPrefab; // Prefab-ul pentru TextMeshPro
    public bool defaultImageActive = false;  // Statusul imaginii default
    public bool copiiImageActive = false;  // Statusul pentru copiile imaginii
    private int numarMaxim = 10; // Numărul maxim de copii al imaginii
    private bool primaInteractiune = false; // Verificăm dacă este prima interacțiune

    // Lista pentru a ține evidența copiilor
    private List<Image> imaginiCopie = new List<Image>();

    void Start()
    {
        // Dezactivăm imaginea principală la început
        imaginePrefab.enabled = defaultImageActive;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (primaInteractiune)
        {
            // Dacă este prima interacțiune, activăm imaginea principală
            imaginePrefab.enabled = defaultImageActive;
            primaInteractiune = false; // Setăm că nu mai este prima interacțiune
        }
        else if (imaginiCopie.Count < numarMaxim)
        {
            // Creăm o nouă instanță a imaginii
            Image nouaImagine = Instantiate(imaginePrefab, transform); // Setăm transformul părinte ca fiind acest obiect

            // Setăm statusul imaginii în funcție de configurația dorită
            nouaImagine.enabled = copiiImageActive;

            nouaImagine.rectTransform.position = eventData.position;

            // Adăugăm imaginea nou creată în lista de imagini copie
            imaginiCopie.Add(nouaImagine);

            // Setăm numărul corespunzător pentru această imagine
            int numarCurent = imaginiCopie.Count; // Numărul curent este dat de numărul de imagini copie

            // Creăm o nouă instanță a TextMeshPro și îl setăm ca fiu al imaginii
            TextMeshProUGUI textMeshPro = Instantiate(textMeshProPrefab, nouaImagine.transform);
            textMeshPro.text = numarCurent.ToString();
        }
        else
        {
            // Dacă s-a ajuns la numărul maxim și este un nou clic, resetăm procesul
            ResetMarkerPlacer();
        }
    }

    private void Update()
    {
        // Iterăm prin toate imaginile copie și actualizăm poziția textului
        foreach (Image imagine in imaginiCopie)
        {
            // Găsim copia textului corespunzătoare acestei imagini
            TextMeshProUGUI textMeshPro = imagine.GetComponentInChildren<TextMeshProUGUI>();

            // Asigurăm că avem un textMeshPro asociat acestei imagini
            if (textMeshPro != null)
            {
                // Preluăm poziția de pe axa X a imaginii părinte și o asignăm textului
                Vector3 pozitieText = textMeshPro.rectTransform.position;
                pozitieText.x = imagine.rectTransform.position.x;
                textMeshPro.rectTransform.position = pozitieText;
            }
        }
    }

    // Metodă pentru a reseta procesul și a șterge copiile
    private void ResetMarkerPlacer()
    {
        // Distrugem toate imaginile copie
        foreach (Image imagine in imaginiCopie)
        {
            // Setăm statusul individual pentru fiecare copie
            imagine.enabled = copiiImageActive;

            Destroy(imagine.gameObject);
        }

        // Golim lista de imagini copie
        imaginiCopie.Clear();
    }
} 