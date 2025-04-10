using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public GameObject enemyBodyModel;
    private Animator animator;
    public float wanderRadius = 10f;
    public float wanderTimer = 5f;
    public float detectionRange = 10f;
    public Transform player;
    public float attackRange = 3f;

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

        if (distance <= attackRange)
        {
            agent.SetDestination(transform.position); // Stop movement
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsAttacking", true);
        }
        else if (distance <= detectionRange)
        {
            agent.isStopped = false;
            agent.speed = 6f;
            agent.SetDestination(player.position);

            animator.SetBool("IsDetected", true);
            animator.SetBool("IsRunning", true);
            animator.SetBool("IsAttacking", false);
        }
        else
        {
            // Optional wander behavior
            animator.SetBool("IsDetected", false);
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsAttacking", false);
        }

        animator.SetFloat("Speed", agent.velocity.magnitude);
    }
}
