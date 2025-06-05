using UnityEngine;
using System.Collections;

public class LockedSlidingDoor : MonoBehaviour
{
    public Transform slidingPart;
    public Vector3 openOffset = new Vector3(0, 0, 2);
    public float slideSpeed = 2f;
    public float autoCloseDelay = 3f;

    public AudioClip openSound;
    public AudioClip closeSound;

    private Vector3 closedPos;
    private Vector3 openPos;
    [HideInInspector] public bool isOpen = false;

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
        if (isOpen)
        {
            slidingPart.position = Vector3.MoveTowards(slidingPart.position, openPos, slideSpeed * Time.deltaTime);

            if (autoCloseCoroutine != null)
                StopCoroutine(autoCloseCoroutine);

            autoCloseCoroutine = StartCoroutine(AutoCloseAfterDelay());
        }
        else
        {
            slidingPart.position = Vector3.MoveTowards(slidingPart.position, closedPos, slideSpeed * Time.deltaTime);
        }
    }

    IEnumerator AutoCloseAfterDelay()
    {
        yield return new WaitForSeconds(autoCloseDelay);
        isOpen = false;

        if (audioSource && closeSound)
            audioSource.PlayOneShot(closeSound);
    }

    public void TriggerOpen()
    {
        isOpen = true;
        if (audioSource && openSound)
            audioSource.PlayOneShot(openSound);
    }
}
