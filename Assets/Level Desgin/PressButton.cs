using UnityEngine;
using TMPro;

public class PressButton : MonoBehaviour
{
    [Header("Doors to Open")]
    public LockedSlidingDoor[] doors; // Array of doors

    [Header("Interaction")]
    public TextMeshProUGUI interactionPrompt;
    public KeyCode interactKey = KeyCode.E;

    private bool isPlayerNear = false;
    private bool doorsOpened = false;

    void Start()
    {
        if (interactionPrompt != null)
            interactionPrompt.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isPlayerNear && !doorsOpened && Input.GetKeyDown(interactKey))
        {
            doorsOpened = true;

            foreach (var door in doors)
            {
                if (door != null)
                    door.TriggerOpen();
            }

            if (interactionPrompt != null)
                interactionPrompt.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            if (!doorsOpened && interactionPrompt != null)
                interactionPrompt.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            if (interactionPrompt != null)
                interactionPrompt.gameObject.SetActive(false);
        }
    }
}
