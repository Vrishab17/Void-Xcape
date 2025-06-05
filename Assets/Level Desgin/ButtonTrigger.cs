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
