using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    public InventorySlot[] slots;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public bool Add(InventoryItem item)
    {
        foreach (var slot in slots)
        {
            // Add to first empty slot
            if (slot.GetComponent<InventorySlot>().icon.enabled == false)
            {
                slot.GetComponent<InventorySlot>().AddItem(item);
                return true;
            }
        }
        Debug.Log("Inventory Full!");
        return false;
    }
}
