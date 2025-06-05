using System.Collections;
using TMPro;
using UnityEngine;

public class MultiTypewriter : MonoBehaviour
{
    [System.Serializable]
    public class TypingBlock
    {
        public TextMeshProUGUI textUI;
        [TextArea]
        public string fullText;
    }

    public TypingBlock[] blocks;
    public float typingSpeed = 0.05f;
    public float waitBetweenBlocks = 1f;
    public GameObject continueButton;
    public AudioClip beepSound;
    private AudioSource audioSource;

    private bool skipRequested = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(TypeAllBlocks());
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            skipRequested = true;
        }
    }

    IEnumerator TypeAllBlocks()
    {
        foreach (var block in blocks)
        {
            skipRequested = false;
            yield return StartCoroutine(TypeText(block));
            yield return new WaitForSeconds(waitBetweenBlocks);
        }

        continueButton.SetActive(true);
    }


    IEnumerator TypeText(TypingBlock block)
{
    block.textUI.text = "";

    for (int i = 0; i < block.fullText.Length; i++)
    {
        if (skipRequested)
        {
            block.textUI.text = block.fullText;
            yield break;
        }

        block.textUI.text += block.fullText[i];

        if (beepSound && audioSource && !char.IsWhiteSpace(block.fullText[i]))
        {
            audioSource.PlayOneShot(beepSound);
        }

        yield return new WaitForSeconds(typingSpeed);
    }
}
}
