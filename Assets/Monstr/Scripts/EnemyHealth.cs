using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    private Animator animator;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;

        animator = GetComponentInChildren<Animator>();
        if (animator == null)
        {
            Debug.LogWarning("Animator not found on enemy.");
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null) agent.isStopped = true;

        Collider collider = GetComponent<Collider>();
        if (collider != null) collider.enabled = false;

        
        Destroy(gameObject, 3f);
    }
}
