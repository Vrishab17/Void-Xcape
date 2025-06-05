using UnityEngine;
using System.Collections;

public class SlidingDoor : MonoBehaviour
{
    public Transform slidingPart;
    public Vector3 openOffset = new Vector3(0, 0, 2);
    public float slideSpeed = 2f;
    public float autoCloseDelay = 3f;

    public GameObject interactionPrompt;

    public AudioClip openSound;
    public AudioClip closeSound;

    private Vector3 closedPos;
    private Vector3 openPos;
    private bool isPlayerNear = false;
    private bool isOpen = false;
    private Coroutine autoCloseCoroutine;

    private AudioSource audioSource;

    void Start()
    {
        closedPos = slidingPart.position;
        openPos = closedPos + openOffset;

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            isOpen = true;

            if (audioSource && openSound)
                audioSource.PlayOneShot(openSound);

            if (autoCloseCoroutine != null)
                StopCoroutine(autoCloseCoroutine);

            autoCloseCoroutine = StartCoroutine(AutoCloseAfterDelay());
        }

        if (isOpen)
            slidingPart.position = Vector3.MoveTowards(slidingPart.position, openPos, slideSpeed * Time.deltaTime);
        else
            slidingPart.position = Vector3.MoveTowards(slidingPart.position, closedPos, slideSpeed * Time.deltaTime);
    }

    IEnumerator AutoCloseAfterDelay()
    {
        yield return new WaitForSeconds(autoCloseDelay);
        isOpen = false;

        if (audioSource && closeSound)
            audioSource.PlayOneShot(closeSound);
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
