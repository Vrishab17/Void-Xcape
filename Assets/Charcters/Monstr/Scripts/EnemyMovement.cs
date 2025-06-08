using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    public GameObject enemyBodyModel;
    public Transform player;
    public int coinValue = 5;
    private PlayerHealth playerHealth;
    private Animator animator;
    private NavMeshAgent agent;

    [Header("Detection Settings")]
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float giveUpRange = 20f;

    [Header("Movement Settings")]
    public float runSpeed = 6f;
    public float walkSpeed = 2f;
    public float acceleration = 80f;

    [Header("Attack Settings")]
    public float damageAmount = 10f;

    [Header("Wander Settings")]
    public float wanderRadius = 15f;
    public float wanderInterval = 5f;

    private float wanderTimer;
    private bool isChasing = false;
    private bool isDead = false;
    private bool isAttackingState = false;

    public int health = 100;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = walkSpeed;
        agent.acceleration = acceleration;
        agent.angularSpeed = 999f;
        agent.autoBraking = false;

        if (enemyBodyModel != null)
            animator = enemyBodyModel.GetComponent<Animator>();

        if (player != null)
            playerHealth = player.GetComponent<PlayerHealth>();

        wanderTimer = wanderInterval;
        SetRandomDestination();
    }

    private void Update()
    {
        if (player == null || animator == null || isDead)
            return;

        float distance = Vector3.Distance(transform.position, player.position);
        bool inAttackRange = distance <= attackRange;

        // 🔄 Fix: Update IsAttacking before exiting
        if (!inAttackRange && animator.GetBool("IsAttacking"))
        {
            animator.SetBool("IsAttacking", false);
        }

        // Stop movement and animation if attacking
        if (animator.GetBool("IsAttacking"))
        {
            if (!isAttackingState)
            {
                isAttackingState = true;

                if (agent.enabled)
                {
                    agent.isStopped = true;
                    agent.velocity = Vector3.zero;
                    agent.enabled = false;
                }

                Rigidbody rb = GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    rb.isKinematic = true;
                }

                animator.SetFloat("Speed", 0f);
                animator.SetBool("IsRunning", false);
            }
        }
        else
        {
            if (isAttackingState)
            {
                isAttackingState = false;

                if (!agent.enabled)
                {
                    agent.enabled = true;
                    agent.isStopped = false;
                }

                Rigidbody rb = GetComponent<Rigidbody>();
                if (rb != null)
                    rb.isKinematic = false;

                animator.SetBool("IsRunning", true);
            }

            if (!isDead && agent.enabled)
            {
                animator.SetFloat("Speed", agent.velocity.magnitude);
            }
        }

        if (!agent.enabled || !agent.isOnNavMesh) return;

        if (inAttackRange && !animator.GetBool("IsAttacking"))
        {
            animator.SetBool("IsAttacking", true);
            isChasing = true;

            Vector3 lookDir = (player.position - transform.position).normalized;
            lookDir.y = 0f;
            Quaternion lookRot = Quaternion.LookRotation(lookDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 10f);
        }
        else if (distance <= detectionRange)
        {
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            Ray ray = new Ray(transform.position + Vector3.up * 1.5f, directionToPlayer);

            if (Physics.Raycast(ray, out RaycastHit hit, detectionRange))
            {
                if (hit.transform == player)
                {
                    isChasing = true;
                    agent.isStopped = false;
                    agent.speed = runSpeed;
                    agent.SetDestination(player.position);
                }
            }
        }
        else if (isChasing && distance > giveUpRange)
        {
            isChasing = false;
            SetRandomDestination();
        }

        if (!isChasing && !animator.GetBool("IsAttacking"))
        {
            wanderTimer -= Time.deltaTime;

            if (!agent.pathPending && agent.remainingDistance < 0.5f || wanderTimer <= 0f)
            {
                SetRandomDestination();
                wanderTimer = wanderInterval;
            }
        }
    }

    private void SetRandomDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += transform.position;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, wanderRadius, NavMesh.AllAreas))
        {
            agent.isStopped = false;
            agent.speed = walkSpeed;
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

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        agent.enabled = false;
        animator.SetFloat("Speed", 0f);

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = false;

        animator.SetTrigger("Die");

        if (CoinManager.Instance != null)
            CoinManager.Instance.AddCoins(coinValue);

        Destroy(gameObject, 3f);
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
            Die();
    }
}
