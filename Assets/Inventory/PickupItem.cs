using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public InventoryItem item;
    public int amount = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bool added = Inventory.Instance.Add(item, amount);
            if (added) Destroy(gameObject);
        }
    }
}
