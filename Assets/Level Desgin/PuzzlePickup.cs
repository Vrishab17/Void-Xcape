using UnityEngine;

public class PuzzleCollectible : MonoBehaviour
{
    [Header("Puzzle Item")]
    public string itemID = "Item1"; 

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        Debug.Log("Collected puzzle item: " + itemID);

        // Count toward door unlock
        if (ItemCollectionTracker.Instance != null)
        {
            ItemCollectionTracker.Instance.RegisterItemCollected();
        }

        Destroy(gameObject);
    }
}
