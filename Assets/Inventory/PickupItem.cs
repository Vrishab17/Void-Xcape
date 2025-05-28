using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PickupItem : MonoBehaviour
{
    public InventoryItem item;
    public int amount = 1;
    public AudioClip pickupSound;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bool added = Inventory.Instance.Add(item, amount);
            if (added)
            {
                if (pickupSound != null)
                {
                    audioSource.PlayOneShot(pickupSound);
                    Destroy(gameObject, pickupSound.length);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
