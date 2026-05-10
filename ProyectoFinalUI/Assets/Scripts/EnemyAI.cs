using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Stats")]
    public float health = 100f;
    public float damageAmount = 20f;
    public float attackCooldown = 1.5f;
    public float chaseRange = 10f;
    public float attackRange = 2f;

    [Header("References")]
    private Transform player;
    private NavMeshAgent agent;
    private PlayerController playerScript;
    private float nextAttackTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        // Buscamos al jugador por su Tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerScript = playerObj.GetComponent<PlayerController>();
        }
    }

    void Update()
    {
        if (player == null || playerScript.isDead) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= chaseRange)
        {
            // Perseguir
            agent.SetDestination(player.position);

            // Atacar si está lo suficientemente cerca
            if (distanceToPlayer <= attackRange && Time.time >= nextAttackTime)
            {
                AttackPlayer();
            }
        }
    }

    void AttackPlayer()
    {
        nextAttackTime = Time.time + attackCooldown;
        playerScript.TakeDamage(damageAmount);
        Debug.Log("El enemigo te ha golpeado");
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        Debug.Log("Enemigo recibe daño. Vida restante: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Efecto simple de muerte
        Debug.Log("Enemigo derrotado");
        Destroy(gameObject);
    }
}