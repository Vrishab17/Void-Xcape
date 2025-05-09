using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class InventoryItem : ScriptableObject
{
    public string itemName;
    public int maxStack = 99;
    public Sprite icon;
    public bool isStackable;
    

    public virtual void Use()
    {
        Debug.Log("Item Used: " + itemName);
    }
}