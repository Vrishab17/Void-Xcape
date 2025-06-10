using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    public CanvasGroup fadeGroup;
    public GameObject storyPanel; 
    public string sceneToLoad = "Level 1";
    public bool loadNextSceneByIndex = false;


    [Header("Fade Durations")]
    public float fadeInDuration = 1f;
    public float fadeOutDuration = 1f;

    [Header("Fade Delays")]
    public float fadeInDelay = 0f;
    public float fadeOutDelay = 0f;

    void Start()
    {
        if (fadeGroup.alpha == 1)
        {
            StartCoroutine(FadeIn());
        }
    }

    public void FadeToScene()
    {
        if (storyPanel != null)
            storyPanel.SetActive(false); 

        StartCoroutine(FadeOutAndLoad());
    }

    IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(fadeInDelay); 

        fadeGroup.blocksRaycasts = true;
        float t = 0f;
        while (t < fadeInDuration)
        {
            t += Time.deltaTime;
            fadeGroup.alpha = 1 - Mathf.Clamp01(t / fadeInDuration);
            yield return null;
        }

        fadeGroup.alpha = 0f;
        fadeGroup.blocksRaycasts = false;
    }

    IEnumerator FadeOutAndLoad()
    {
        yield return new WaitForSeconds(fadeOutDelay); 

        fadeGroup.blocksRaycasts = true;
        float t = 0f;
        while (t < fadeOutDuration)
        {
            t += Time.deltaTime;
            fadeGroup.alpha = Mathf.Clamp01(t / fadeOutDuration);
            yield return null;
        }

        fadeGroup.alpha = 1f;
        if (loadNextSceneByIndex)
        {
            int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
            SceneManager.LoadScene(nextIndex);
        }
        else
        {
            SceneManager.LoadScene(sceneToLoad);
        }

    }
}
