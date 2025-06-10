using UnityEngine;

public class ItemCollectionTracker : MonoBehaviour
{
    public static ItemCollectionTracker Instance;

    public int requiredCount = 4;
    private int collectedCount = 0;

    private ObjectiveManager manager;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        // Cache the reference once at the beginning
        manager = GameObject.FindFirstObjectByType<ObjectiveManager>();
    }

    public void RegisterItemCollected()
    {
        collectedCount++;
        Debug.Log($"Puzzle Items: {collectedCount}/{requiredCount}");

        // Update the objective UI only if the manager is valid
        if (manager != null && manager.GetCurrentIndex() == 0)
        {
            ObjectiveSlot slot = manager.GetCurrentSlot();
            if (slot != null && slot.objectiveText != null)
            {
                slot.objectiveText.text = $"Find key cards ({collectedCount}/{requiredCount})";
            }

            // Complete the objective if done
            if (collectedCount >= requiredCount)
            {
                manager.CompleteCurrentObjective();
            }
        }
        else if (manager == null)
        {
            Debug.LogWarning("ObjectiveManager not found in scene.");
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
