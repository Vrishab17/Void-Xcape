using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{
    public LockedSlidingDoor lockedDoor; // Assign in inspector

    private int objectsOnButton = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickup"))
        {
            objectsOnButton++;
            if (lockedDoor != null)
                lockedDoor.TriggerOpen();

            // Check if this is the "Solve the puzzle" objective (e.g., index 3)
            ObjectiveManager mgr = FindFirstObjectByType<ObjectiveManager>();
            if (mgr != null && mgr.GetCurrentIndex() == 3)
            {
                mgr.CompleteCurrentObjective();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pickup"))
        {
            objectsOnButton--;
            if (objectsOnButton <= 0 && lockedDoor != null)
                lockedDoor.isOpen = false;
        }
    }
}
