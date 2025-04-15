using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    public GameObject enemyBodyModel;
    public Transform player;
    private PlayerHealth playerHealth;
    private Animator animator;
    private NavMeshAgent agent;

    [Header("Detection Settings")]
    public float detectionRange = 10f;
    public float attackRange = 2f;

    [Header("Attack Settings")]
    public float damageAmount = 10f;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (enemyBodyModel != null)
            animator = enemyBodyModel.GetComponent<Animator>();

        if (player != null)
            playerHealth = player.GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        if (player == null || animator == null || !agent.isOnNavMesh)
            return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            // Stop & attack
            agent.SetDestination(transform.position);
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsAttacking", true);
        }
        else if (distance <= detectionRange)
        {
            // Chase
            agent.isStopped = false;
            agent.speed = 6f;
            agent.SetDestination(player.position);

            animator.SetBool("IsDetected", true);
            animator.SetBool("IsRunning", true);
            animator.SetBool("IsAttacking", false);
        }
        else
        {
            // Idle
            animator.SetBool("IsDetected", false);
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsAttacking", false);
        }

        animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    // ✅ Called by animation event at the exact hit frame
    public void DealDamageToPlayer()
    {
        if (playerHealth == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= attackRange)
        {
            playerHealth.TakeDamage(damageAmount);
        }
    }public void DealDamage2()
    {
        if (playerHealth == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= attackRange)
        {
            playerHealth.TakeDamage(damageAmount);
        }
    }
}
