using UnityEngine;

public class GunPickup : MonoBehaviour
{
    public GameObject playerGun; // The gun in the player's hands
    
    [Header("Audio Settings")]
    [SerializeField] private AudioClip pickupClip; // Pickup sound effect
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Play pickup sound before destroying
            if (pickupClip != null && SFXManager.instance != null)
            {
                SFXManager.instance.PlayClip(pickupClip, transform);
            }
            
            if (playerGun != null)
                playerGun.SetActive(true); // Show the held gun
                
            Destroy(gameObject); // Remove pickup model from floor
        }
    }
}