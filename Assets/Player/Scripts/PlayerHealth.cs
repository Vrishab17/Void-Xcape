using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;
    private bool isDead = false;

    [Header("UI")]
    public Slider healthSlider;
    public GameObject deathScreen;

    [Header("Respawn Settings")]
    public float respawnDelay = 3f;
    public Transform respawnPoint;

    private Vector3 defaultPosition;
    private Quaternion defaultRotation;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();

        if (deathScreen != null)
            deathScreen.SetActive(false);

        if (respawnPoint == null)
        {
            defaultPosition = transform.position;
            defaultRotation = transform.rotation;
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        UpdateHealthUI();

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        if (isDead) return;

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        if (healthSlider != null)
            healthSlider.value = currentHealth / maxHealth;
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("Player Died");

        MonoBehaviour[] components = GetComponents<MonoBehaviour>();
        foreach (var comp in components)
        {
            if (comp != this)
                comp.enabled = false;
        }

        if (deathScreen != null)
            deathScreen.SetActive(true);

        Invoke("Respawn", respawnDelay);
    }

    void Respawn()
    {
        isDead = false;

        currentHealth = maxHealth;
        UpdateHealthUI();

        if (deathScreen != null)
            deathScreen.SetActive(false);

        MonoBehaviour[] components = GetComponents<MonoBehaviour>();
        foreach (var comp in components)
        {
            comp.enabled = true;
        }

        // Move to respawn point
        if (respawnPoint != null)
        {
            transform.position = respawnPoint.position;
            transform.rotation = respawnPoint.rotation;
        }
        else
        {
            transform.position = defaultPosition;
            transform.rotation = defaultRotation;
        }

        Debug.Log("Player Respawned");
    }

    void OnValidate()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        UpdateHealthUI();
    }
}
