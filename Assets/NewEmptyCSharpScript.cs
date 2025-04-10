using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    public GameObject enemyBodyModel;
    private Animator animator;
    public float wanderRadius = 10f;
    public float wanderTimer = 5f;
    public float detectionRange = 10f;
    public Transform player;
    public float attackRange = 3f; // The range at which the attack starts
    public float safeDistanceFromPlayer = 2.5f; // Safe distance from the player for the attack
    private bool hasJumpAttacked = false;

    private NavMeshAgent agent;
    private float timer;

    void Start()
    {
        if (enemyBodyModel != null)
        {
            animator = enemyBodyModel.GetComponent<Animator>();
        }
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;
    }

    void Update()
    {
        if (!agent.isOnNavMesh || player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // If the player is within attack range, initiate attack
        if (distance <= attackRange)
        {
            // Stop moving to allow the attack to start
            agent.SetDestination(transform.position);  // Stop the agent's movement

            // Ensure the enemy is at a safe distance in front of the player, not inside them
            if (!hasJumpAttacked)
            {
                Vector3 forwardDirection = player.forward; // Get the player's forward direction
                Vector3 attackPosition = player.position + forwardDirection * safeDistanceFromPlayer; // Position in front of the player
                attackPosition.y = transform.position.y; // Match the Y-axis to avoid height issues

                // Move the enemy to the attack position (in front of the player)
                agent.SetDestination(attackPosition);

                // Trigger jump attack animation
                animator.SetBool("IsAttacking", true);
                animator.SetInteger("AttackStage", 1); // Trigger jump attack
                hasJumpAttacked = true;

                // Optionally start coroutine to switch to default attack after jump
                StartCoroutine(SwitchToDefaultAttack());
            }
            else
            {
                // Trigger default attack animation once the jump attack is done
                animator.SetBool("IsAttacking", true);
                animator.SetInteger("AttackStage", 2); // Default attack
            }
        }
        else
        {
            // Reset attack status
            hasJumpAttacked = false;
            animator.SetBool("IsAttacking", false);
            animator.SetInteger("AttackStage", 0);

            // Continue chasing the player if they are in detection range
            agent.isStopped = false; // Allow movement again
            agent.speed = 6f;
            agent.SetDestination(player.position);

            // Set running animation
            animator.SetBool("IsRunning", true);
            animator.SetBool("IsWalking", false);
        }

        // Animation speed for movement
        animator.SetFloat("Speed", agent.velocity.magnitude);

        // If the player is in detection range, chase
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            // If player is detected, chase the player
            animator.SetBool("IsDetected", true);
        }
        else
        {
            // If not detected, start wandering behavior
            animator.SetBool("IsDetected", false);
            agent.speed = 2f; // Walk speed
            timer += Time.deltaTime;

            if (timer >= wanderTimer)
            {
                Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
                agent.SetDestination(newPos);
                timer = 0;
            }

            // Set walking animation
            animator.SetBool("IsWalking", true);
            animator.SetBool("IsRunning", false);
        }
    }

    // Coroutine to switch to default attack animation after jump attack
    IEnumerator SwitchToDefaultAttack()
    {
        yield return new WaitForSeconds(1.2f); // Adjust time to match jump attack duration
        animator.SetInteger("AttackStage", 2); // Switch to default attack
    }

    // Helper method to generate random navigation points
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;

        NavMesh.SamplePosition(randDirection, out NavMeshHit navHit, dist, layermask);

        return navHit.position;
    }
}
