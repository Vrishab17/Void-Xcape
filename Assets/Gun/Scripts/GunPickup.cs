using UnityEngine;

public class GunPickup : MonoBehaviour
{
    public GameObject playerGun; // The gun in the player's hands
    
    [Header("Audio Settings")]
    [SerializeField] private AudioClip PICKUPClip; // Pickup sound effect
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Play pickup sound before destroying
            if (PICKUPClip != null && SFXManager.instance != null)
            {
                SFXManager.instance.PlayClip(PICKUPClip, transform);
            }
            
            if (playerGun != null)
                playerGun.SetActive(true); // Show the held gun
                
            Destroy(gameObject); // Remove pickup model from floor
        }
    }
}