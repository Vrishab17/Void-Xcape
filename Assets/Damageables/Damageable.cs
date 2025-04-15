using UnityEngine;
using UnityEngine.Rendering;

public class Damageable : MonoBehaviour
{
    [SerializeField] float maxHealth = 100f;
    float currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Overloaded method (optional if you're passing hit info)
    public void TakeDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        // Add VFX, hit direction logic, etc.
        TakeDamage(damage); // call the other method
    }

    void Die()
    {
        print(name + " has died.");
        Destroy(gameObject);
    }
}

