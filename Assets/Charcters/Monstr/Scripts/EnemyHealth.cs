using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    public DamageTextSpawner textSpawner;

    public int coinValue = 5;
    private Animator animator;
    public bool isDead = false;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip HITClip;
    [SerializeField] private AudioClip DEADClip;
    private static int enemiesKilled = 0;

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

        // Play hit sound
        if (SFXManager.instance != null && HITClip != null)
        {
            SFXManager.instance.PlayClip(HITClip, transform);
        }

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
        if (isDead) return;
        isDead = true;

        // Play death sound
        if (SFXManager.instance != null && DEADClip != null)
        {
            SFXManager.instance.PlayClip(DEADClip, transform);
        }

        // Stop NavMeshAgent if present
        var agent = GetComponent<NavMeshAgent>();
        if (agent != null && agent.isOnNavMesh)
        {
            agent.isStopped = true;
        }

        // Disable collider
        var collider = GetComponent<Collider>();
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

        // Objective handling
        var mgr = FindFirstObjectByType<ObjectiveManager>();
        if (mgr != null)
        {
            if (!CompareTag("Boss"))
            {
                enemiesKilled++;

                if (mgr.GetCurrentIndex() == 1)
                {
                    var current = mgr.GetCurrentObjective();
                    var slot = mgr.GetCurrentSlot();

                    if (current != null && slot != null)
                    {
                        slot.objectiveText.text = $"{current.description} ({enemiesKilled}/6)";
                    }

                    if (enemiesKilled >= 6)
                    {
                        mgr.CompleteCurrentObjective();
                    }
                }
            }
            else if (CompareTag("Boss") && mgr.GetCurrentIndex() == 2)
            {
                mgr.CompleteCurrentObjective();
            }
        }

        // Destroy enemy after delay
        Destroy(gameObject, 3f);
    }
}
