using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public InventoryItem item;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bool added = Inventory.Instance.Add(item);
            if (added) Destroy(gameObject);
        }
    }
}
