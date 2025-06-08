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
    private EnemyHealth enemyHealth; // NEW: Reference to EnemyHealth

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
    private bool wasChasing = false; // Track previous state for audio triggers

    [Header("Audio Settings")]
    [SerializeField] private AudioClip WALKClip;
    [SerializeField] private AudioClip YELLClip;
    [SerializeField] private AudioClip ATTACKClip; // NEW: Attack sound
    [SerializeField] private float walkStepInterval = 0.5f;
    [SerializeField] private float chaseStepInterval = 0.3f; // Faster steps when chasing
    [SerializeField] private float yellInterval = 3f; // Time between yells
    
    private float walkStepTimer = 0f;
    private float yellTimer = 0f;
    private bool hasYelledOnDetection = false;
    private bool isCurrentlyAttacking = false; // NEW: Track attack state

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyHealth = GetComponent<EnemyHealth>(); // NEW: Get EnemyHealth reference

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

        // NEW: Check if enemy is dead - stop all behavior if dead
        if (enemyHealth != null && enemyHealth.isDead)
            return;

        float distance = Vector3.Distance(transform.position, player.position);
        wasChasing = isChasing; // Store previous state
        bool wasAttacking = isCurrentlyAttacking; // Store previous attack state

        if (distance <= attackRange)
        {
            isChasing = true;
            isCurrentlyAttacking = true; // NEW: Set attack state
            agent.SetDestination(transform.position);
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsAttacking", true);
            animator.SetBool("IsWalking", false);
        }
        else if (distance <= detectionRange)
        {
            isChasing = true;
            isCurrentlyAttacking = false; // NEW: Not attacking when chasing
            agent.isStopped = false;
            agent.speed = 6f;
            agent.SetDestination(player.position);

            animator.SetBool("IsDetected", true);
            animator.SetBool("IsRunning", true);
            animator.SetBool("IsAttacking", false);
            animator.SetBool("IsWalking", false);

            // Trigger yell when first detecting player
            if (!wasChasing && !hasYelledOnDetection)
            {
                TriggerYell();
                hasYelledOnDetection = true;
                yellTimer = yellInterval; // Set timer for next yell
            }
        }
        else if (isChasing && distance > giveUpRange)
        {
            isChasing = false;
            isCurrentlyAttacking = false; // NEW: Reset attack state
            hasYelledOnDetection = false; // Reset for next detection
            SetRandomDestination();
        }

        if (!isChasing)
        {
            isCurrentlyAttacking = false; // NEW: Reset attack state when not chasing
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

            // Reset audio timers when not chasing
            hasYelledOnDetection = false;
            yellTimer = 0f;
        }

        animator.SetFloat("Speed", agent.velocity.magnitude);

        // Handle Audio
        HandleMovementAudio();
        HandleYellAudio();
    }

    private void HandleMovementAudio()
    {
        // NEW: Don't play audio if enemy is dead
        if (enemyHealth != null && enemyHealth.isDead)
            return;

        // Check if enemy is moving (agent velocity > small threshold)
        bool isMoving = agent.velocity.magnitude > 0.1f;

        if (isMoving && WALKClip != null && SFXManager.instance != null)
        {
            // Set step interval based on movement state
            float currentStepInterval = isChasing ? chaseStepInterval : walkStepInterval;
            
            walkStepTimer -= Time.deltaTime;

            if (walkStepTimer <= 0f)
            {
                SFXManager.instance.PlayClip(WALKClip, transform);
                walkStepTimer = currentStepInterval;
            }
        }
        else
        {
            // Reset timer when not moving to avoid clip spam
            walkStepTimer = 0f;
        }
    }

    private void HandleYellAudio()
    {
        // NEW: Don't play audio if enemy is dead
        if (enemyHealth != null && enemyHealth.isDead)
            return;

        // Handle periodic yelling while chasing
        if (isChasing && YELLClip != null && SFXManager.instance != null)
        {
            yellTimer -= Time.deltaTime;

            if (yellTimer <= 0f)
            {
                TriggerYell();
                yellTimer = yellInterval; // Reset timer
            }
        }
    }

    private void TriggerYell()
    {
        // NEW: Don't play audio if enemy is dead
        if (enemyHealth != null && enemyHealth.isDead)
            return;

        if (YELLClip != null && SFXManager.instance != null)
        {
            SFXManager.instance.PlayClip(YELLClip, transform);
        }
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
            SFXManager.instance.PlayClip(ATTACKClip, transform);
        }
    }

}