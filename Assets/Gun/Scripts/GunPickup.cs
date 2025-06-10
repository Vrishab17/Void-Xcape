using UnityEngine;

public class GunPickup : MonoBehaviour
{
    public GameObject playerGun; // The gun in the player's hands

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerGun != null)
                playerGun.SetActive(true); // Show the held gun

            Destroy(gameObject); // Remove pickup model from floor
        }
    }
}
