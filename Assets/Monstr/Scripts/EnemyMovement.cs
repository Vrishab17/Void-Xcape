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
    public float giveUpRange = 20f;

    [Header("Attack Settings")]
    public float damageAmount = 10f;

    [Header("Wander Settings")]
    public float wanderRadius = 15f;
    public float wanderInterval = 5f;

    private float wanderTimer;
    private bool isChasing = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (enemyBodyModel != null)
            animator = enemyBodyModel.GetComponent<Animator>();

        if (player != null)
            playerHealth = player.GetComponent<PlayerHealth>();

        wanderTimer = wanderInterval;
        SetRandomDestination();
    }

    private void Update()
    {
        if (player == null || animator == null || !agent.isOnNavMesh)
            return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            isChasing = true;
            agent.SetDestination(transform.position);
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsAttacking", true);
            animator.SetBool("IsWalking", false);
        }
        else if (distance <= detectionRange)
        {
            isChasing = true;
            agent.isStopped = false;
            agent.speed = 6f;
            agent.SetDestination(player.position);

            animator.SetBool("IsDetected", true);
            animator.SetBool("IsRunning", true);
            animator.SetBool("IsAttacking", false);
            animator.SetBool("IsWalking", false);
        }
        else if (isChasing && distance > giveUpRange)
        {
            isChasing = false;
            SetRandomDestination();
        }

        if (!isChasing)
        {
            wanderTimer -= Time.deltaTime;

            if (!agent.pathPending && agent.remainingDistance < 0.5f || wanderTimer <= 0f)
            {
                SetRandomDestination();
                wanderTimer = wanderInterval;
            }

            animator.SetBool("IsDetected", false);
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsAttacking", false);
            animator.SetBool("IsWalking", true);
        }

        animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    private void SetRandomDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += transform.position;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, wanderRadius, NavMesh.AllAreas))
        {
            agent.isStopped = false;
            agent.speed = 2f;
            agent.SetDestination(hit.position);
        }
    }

    public void DealDamageToPlayer()
    {
        if (playerHealth == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= attackRange)
        {
            playerHealth.TakeDamage(damageAmount);
        }
    }

    public void DealDamage2()
    {
        DealDamageToPlayer();
    }
}
