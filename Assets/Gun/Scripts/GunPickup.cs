using UnityEngine;

public class GunPickup : MonoBehaviour
{
    public GameObject playerGun;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerGun != null)
                playerGun.SetActive(true);

            
            ObjectiveManager manager = FindObjectOfType<ObjectiveManager>();
            if (manager != null)
                manager.CompleteCurrentObjective();

            Destroy(gameObject);
        }
    }
}
