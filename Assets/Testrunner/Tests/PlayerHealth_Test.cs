using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthTests
{
    private GameObject playerGameObject;
    private PlayerHealth playerHealth;
    private GameObject healthSliderObject;
    private Slider healthSlider;

    [SetUp]
    public void SetUp()
    {
        // Create player GameObject with PlayerHealth component
        playerGameObject = new GameObject("TestPlayer");
        playerHealth = playerGameObject.AddComponent<PlayerHealth>();
        
        // Create health slider UI
        healthSliderObject = new GameObject("HealthSlider");
        healthSlider = healthSliderObject.AddComponent<Slider>();
        
        // Set up health values
        playerHealth.maxHealth = 100f;
        playerHealth.healthSlider = healthSlider;
        
        // Manually initialize values that Start() would set
        playerHealth.currentHealth = playerHealth.maxHealth;
        
        // Manually update UI that Start() would call
        if (healthSlider != null)
            healthSlider.value = playerHealth.currentHealth / playerHealth.maxHealth;
    }

    [TearDown]
    public void TearDown()
    {
        if (playerGameObject != null)
            Object.DestroyImmediate(playerGameObject);
        if (healthSliderObject != null)
            Object.DestroyImmediate(healthSliderObject);
    }

    [Test]
    public void ViewHealth_InitialHealth_ShouldShowFullHealth()
    {
        // Test that player can view their initial full health
        Assert.AreEqual(100f, playerHealth.currentHealth, "Player should start with full health");
        Assert.AreEqual(1f, healthSlider.value, 0.01f, "Health slider should show full health (1.0)");
    }

    [Test]
    public void ViewHealth_AfterTakingDamage_ShouldShowReducedHealth()
    {
        // Player takes damage
        playerHealth.TakeDamage(25f);
        
        // Test that player can view their reduced health
        Assert.AreEqual(75f, playerHealth.currentHealth, "Health should be reduced to 75");
        Assert.AreEqual(0.75f, healthSlider.value, 0.01f, "Health slider should show 75% health");
    }

    [Test]
    public void ViewHealth_AfterHealing_ShouldShowIncreasedHealth()
    {
        // Player takes damage first
        playerHealth.TakeDamage(40f);
        
        // Then heals
        playerHealth.Heal(20f);
        
        // Test that player can view their updated health
        Assert.AreEqual(80f, playerHealth.currentHealth, "Health should be 80 after healing");
        Assert.AreEqual(0.8f, healthSlider.value, 0.01f, "Health slider should show 80% health");
    }

    [Test]
    public void ViewHealth_AtZeroHealth_ShouldShowEmpty()
    {
        // Player takes fatal damage
        playerHealth.TakeDamage(100f);
        
        // Test that player can view their zero health
        Assert.AreEqual(0f, playerHealth.currentHealth, "Health should be 0 when dead");
        Assert.AreEqual(0f, healthSlider.value, 0.01f, "Health slider should show empty (0.0)");
    }

    [Test]
    public void ViewHealth_HealthSliderValue_ShouldMatchHealthPercentage()
    {
        // Test various health values and their slider representation
        playerHealth.TakeDamage(50f); // 50% health
        Assert.AreEqual(0.5f, healthSlider.value, 0.01f, "50% health should show 0.5 on slider");
        
        playerHealth.TakeDamage(25f); // 25% health  
        Assert.AreEqual(0.25f, healthSlider.value, 0.01f, "25% health should show 0.25 on slider");
        
        playerHealth.Heal(75f); // Back to 100% health
        Assert.AreEqual(1f, healthSlider.value, 0.01f, "100% health should show 1.0 on slider");
    }
}