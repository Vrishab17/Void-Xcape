using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    public Transform slidingPart;
    public Vector3 openOffset = new Vector3(0, 0, 2); 
    public float slideSpeed = 2f;

    private Vector3 closedPos;
    private Vector3 openPos;
    private bool isPlayerNear = false;
    private bool isOpen = false;
    public GameObject interactionPrompt;

    void Start()
    {
        closedPos = slidingPart.position;
        openPos = closedPos + openOffset;
    }

    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            isOpen = !isOpen;
        }

        if (isOpen)
            slidingPart.position = Vector3.MoveTowards(slidingPart.position, openPos, slideSpeed * Time.deltaTime);
        else
            slidingPart.position = Vector3.MoveTowards(slidingPart.position, closedPos, slideSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;

            if (interactionPrompt != null)
                interactionPrompt.SetActive(true);
        
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;

            if (interactionPrompt != null)
            interactionPrompt.SetActive(false);
        }
    }
}
