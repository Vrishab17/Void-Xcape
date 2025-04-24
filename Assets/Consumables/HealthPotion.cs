using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    public int healAmount = 25;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Transform holdPoint = other.transform.Find("Camera/PotionHoldPoint"); // Adjust path if needed

            if (holdPoint != null)
            {
                transform.SetParent(holdPoint);
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;

                Collider col = GetComponent<Collider>();
                if (col != null) col.enabled = false;

                Rigidbody rb = GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                    rb.useGravity = false;
                }

                PotionInventory inventory = other.GetComponent<PotionInventory>();
                if (inventory != null)
                {
                    inventory.EquipPotion(gameObject, healAmount);
                }
            }
        }
    }
}


