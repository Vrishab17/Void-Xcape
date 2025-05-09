using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    public int healAmount = 25;

    public void Use()
    {
        PlayerHealth player = FindObjectOfType<PlayerHealth>();
        if (player != null)
        {
            player.Heal(healAmount);
            Debug.Log("Used Health Potion. Healed " + healAmount + " HP.");

            Inventory inventory = FindObjectOfType<Inventory>();
            if (inventory != null)
            {
                inventory.UsePotion();
            }
        }
    }

    public void AddToInventory()
    {
        Inventory inventory = FindObjectOfType<Inventory>();
        if (inventory != null)
        {
            InventoryItem item = GetComponent<InventoryItem>();
            if (item != null)
            {
                inventory.Add(item, 1);
            }
        }
    }
}
