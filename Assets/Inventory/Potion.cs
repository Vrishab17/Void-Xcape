using UnityEngine;

[CreateAssetMenu(fileName = "New Potion", menuName = "Inventory/Potion")]
public class Potion : InventoryItem
{
    public float healAmount = 25f;

    public override void Use()
    {
        Debug.Log("Potion Use() triggered.");

        
        GameObject playerObject = GameObject.Find("FirstPerson");
        if (playerObject == null)
        {
            Debug.LogError("GameObject named 'FirstPerson' not found in scene.");
            return;
        }
        Debug.Log("Found GameObject 'FirstPerson'");

        
        PlayerHealth playerHealth = playerObject.GetComponent<PlayerHealth>();
        if (playerHealth == null)
        {
            Debug.LogError("'FirstPerson' does not have a PlayerHealth component.");
            return;
        }
        Debug.Log("Found PlayerHealth component");

        Debug.Log($"Current Health: {playerHealth.currentHealth} / {playerHealth.maxHealth}");

        
        if (playerHealth.currentHealth >= playerHealth.maxHealth)
        {
            Debug.Log("Health is full. Potion not used.");
            return;
        }

        
        playerHealth.Heal(healAmount);
        Debug.Log($"Healed player for {healAmount} HP!");
    }
}
