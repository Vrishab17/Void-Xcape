using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    public DamageTextSpawner textSpawner;

    public int coinValue = 5;
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

    // Show damage number
    if (textSpawner != null)
    {
        Vector3 hitPosition = transform.position + Vector3.up * 2f;
        textSpawner.ShowDamage(amount, hitPosition);
    }

    if (currentHealth <= 0f)
    {
        Die();
    }
}


    void Die()
{
    if (isDead) return; // Prevent multiple deaths

    isDead = true;

    // Stop NavMeshAgent if possible
    UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    if (agent != null && agent.isOnNavMesh)
    {
        agent.isStopped = true;
    }

    // Disable collider
    Collider collider = GetComponent<Collider>();
    if (collider != null)
    {
        collider.enabled = false;
    }

    // Trigger death animation
    if (animator != null)
    {
        animator.SetTrigger("Die");
    }

    // Add coins
    if (CoinManager.Instance != null)
    {
        CoinManager.Instance.AddCoins(coinValue);
    }

    // Delay destroy to let death animation play
    Destroy(gameObject, 3f);
}

}
