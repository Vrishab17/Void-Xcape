using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float maxArmour = 100f;
    public float currentHealth;
    public float currentArmour;
    private bool isDead = false;

    [Header("UI")]
    public Slider healthSlider;
    public Slider armourSlider;
    public GameObject deathScreen;

    [Header("Respawn Settings")]
    public float respawnDelay = 3f;
    public Transform respawnPoint;

    private Vector3 defaultPosition;
    private Quaternion defaultRotation;

    public GameObject crosshair;
    public GameObject healthBar;
    public GameObject miniMap;
    public GameObject ammoCount;


    void Start()
    {
        currentHealth = maxHealth;
        currentArmour = 0f;
        UpdateHealthUI();
        UpdateArmourUI();

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

    if (currentArmour > 0)
    {
        float armourDamage = Mathf.Min(currentArmour, amount);
        currentArmour -= armourDamage;
        amount -= armourDamage;
        UpdateArmourUI();
    }

    if (amount > 0)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        UpdateHealthUI();
    }

    if (currentHealth <= 0f)
    {
        Die();
    }
}
    public void AddArmour(float amount)
{
    if (isDead) return;

    currentArmour += amount;
    currentArmour = Mathf.Clamp(currentArmour, 0f, maxArmour);
    UpdateArmourUI();
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

    void UpdateArmourUI()
{
    if (armourSlider != null)
        armourSlider.value = currentArmour / maxArmour;
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
    
    if (crosshair != null)
    crosshair.SetActive(false);

    if (healthBar != null)
        healthBar.SetActive(false);
    
    if (miniMap != null)
        miniMap.SetActive(false);
    
    if (ammoCount != null)
        ammoCount.SetActive(false);


    // ✅ Unlock and show cursor
    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true;
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

    // ✅ Re-lock and hide cursor
    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;

    if (crosshair != null)
    crosshair.SetActive(true);

    if (healthBar != null)
        healthBar.SetActive(true);

    if (miniMap != null)
        miniMap.SetActive(true);
    
    if (ammoCount != null)
        ammoCount.SetActive(true);


    Debug.Log("Player Respawned");
}


    void OnValidate()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        UpdateHealthUI();
    }

    public void OnRespawnButtonClicked()
{
    if (isDead)
    {
        CancelInvoke(nameof(Respawn));
        Respawn();
    }
}

}
