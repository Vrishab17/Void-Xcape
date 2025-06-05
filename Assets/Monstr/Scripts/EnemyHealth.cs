using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    public int coinValue = 5;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        if (CoinManager.Instance != null)
        {
            CoinManager.Instance.AddCoins(coinValue);
        }
        Destroy(gameObject);
    }
}
