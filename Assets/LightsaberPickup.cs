using UnityEngine;

public class LightsaberPickup : MonoBehaviour
{
    public GameObject playerLightsaber; 

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerLightsaber != null)
                playerLightsaber.SetActive(true); 

            Destroy(gameObject); 
        }
    }
}
