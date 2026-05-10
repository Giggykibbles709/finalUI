using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public float damageAmount = 20f;
    public float damageCooldown = 1.5f; // Para que no quite vida en cada frame
    private float nextDamageTime;

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Time.time >= nextDamageTime)
            {
                PlayerController player = collision.gameObject.GetComponent<PlayerController>();
                player.TakeDamage(damageAmount);
                nextDamageTime = Time.time + damageCooldown;
            }
        }
    }
}