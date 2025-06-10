using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public InventoryItem item;
    public int count;
    public Image icon;
    public Text countText;

    public void SetItem(InventoryItem newItem, int amount)
    {
        item = newItem;
        count = amount;

        if (item != null)
        {
            icon.sprite = item.icon;
            icon.enabled = true;
        }

        UpdateCountText();
    }

    public void AddItem(int amount)
    {
        count += amount;
        UpdateCountText();
    }

    public void RemoveItem(int amount)
    {
        count -= amount;
        if (count <= 0)
        {
            ClearSlot();
        }
        else
        {
            UpdateCountText();
        }
    }

    public void ClearSlot()
    {
        item = null;
        count = 0;

        if (icon != null)
        {
            icon.sprite = null;
            icon.enabled = false;
        }

        if (countText != null)
        {
            countText.text = "";
        }
    }

    public bool HasItem()
    {
        return item != null;
    }

    private void UpdateCountText()
    {
        if (countText != null)
        {
            countText.text = count >= 1 ? count.ToString() : "";
        }
    }


    public void OnSlotClick()
    {
        if (!HasItem() || count <= 0)
        {
            Debug.Log("No item in this slot.");
            return;
        }

        PotionInventory inventory= GameObject.FindObjectOfType<PotionInventory>();
        if (inventory == null)
        {
            Debug.LogWarning("PotionInventory not found in scene!");
            return;
        }

        if (item.GetType() == typeof(Potion))
        {
            inventory.EquipPotion(this);
            Debug.Log("Potion equipped.");
        }
        else
        {
            Debug.Log("Item is not a potion.");
        }
    }
}
