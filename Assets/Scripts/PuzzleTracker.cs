using UnityEngine;

public class ItemCollectionTracker : MonoBehaviour
{
    public static ItemCollectionTracker Instance;

    public int requiredCount = 4;
    private int collectedCount = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void RegisterItemCollected()
    {
        collectedCount++;
        Debug.Log($"Puzzle Items: {collectedCount}/{requiredCount}");

        // Update the objective text after collecting
        ObjectiveManager manager = Object.FindFirstObjectByType<ObjectiveManager>();
        if (manager != null && manager.GetCurrentIndex() == 0)
        {
            ObjectiveSlot slot = manager.GetCurrentSlot();
            if (slot != null)
            {
                slot.objectiveText.text = $"Find key cards ({collectedCount}/{requiredCount})";
            }

            // Complete objective if all items are collected
            if (collectedCount >= requiredCount)
            {
                manager.CompleteCurrentObjective();
            }
        }
    }

    public bool HasAllRequiredItems()
    {
        return collectedCount >= requiredCount;
    }

    public int GetCollectedCount()
    {
        return collectedCount;
    }
}
