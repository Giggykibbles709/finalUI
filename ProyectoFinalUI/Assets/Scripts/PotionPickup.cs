using UnityEngine;

public class PotionPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.potionCount++;
                Debug.Log("Poción recogida. Total: " + player.potionCount);
                Destroy(gameObject); // El objeto desaparece del mapa
            }
        }
    }
}