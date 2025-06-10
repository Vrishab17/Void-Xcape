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
    }

    public bool HasAllRequiredItems()
    {
        return collectedCount >= requiredCount;
    }
}
