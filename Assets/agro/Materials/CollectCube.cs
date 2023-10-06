using UnityEngine;

public class CollectCube : MonoBehaviour
{
    public AudioSource collectSound;

    void OnTriggerEnter(Collider other)
    {
        // Decrement the static variable numarAch01 from GlobalAchievements
        GlobalAchievements.numarAch01 --;
        collectSound.Play();
        Destroy(gameObject);
    }
}
