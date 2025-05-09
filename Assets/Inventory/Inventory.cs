using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    public InventorySlot[] slots;


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public bool Add(InventoryItem item, int amount)
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.HasItem() && slot.item == item && item.isStackable && slot.count < item.maxStack)
            {
                int spaceLeft = item.maxStack - slot.count;
                int toAdd = Mathf.Min(spaceLeft, amount);
                slot.AddItem(toAdd);
                amount -= toAdd;
                if (amount <= 0)
                    return true;
            }
        }

        foreach (InventorySlot slot in slots)
        {
            if (!slot.HasItem())
            {
                int toAdd = Mathf.Min(item.maxStack, amount);
                slot.SetItem(item, toAdd);
                amount -= toAdd;
                if (amount <= 0)
                    return true;
            }
        }

        Debug.Log("Inventory full!");
        return false;
    }

    public void UsePotion()
    {
        foreach (var slot in slots)
        {
            if (slot.HasItem() && slot.item.itemName == "Potion")
            {
                slot.RemoveItem(1);
                Debug.Log("Potion used.");

                PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.Heal(25f);
                }
                else
                {
                    Debug.LogWarning("PlayerHealth not found");
                }
                return;
            }
        }
        Debug.Log("No potion in inventory.");
    }
}