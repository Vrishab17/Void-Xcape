using UnityEngine;

public class PuzzleDoorUnlocker : MonoBehaviour
{
    [Header("Doors to Unlock")]
    public LockedSlidingDoor[] doors;

    private bool hasOpened = false;

    void Update()
    {
        if (hasOpened || ItemCollectionTracker.Instance == null)
            return;

        if (ItemCollectionTracker.Instance.HasAllRequiredItems())
        {
            foreach (LockedSlidingDoor door in doors)
            {
                if (door != null)
                    door.TriggerOpen();
            }

            hasOpened = true;
            Debug.Log("All puzzle items collected. All doors opened.");
        }
    }
}
