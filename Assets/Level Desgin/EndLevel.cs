using UnityEngine;

public class LevelEndTrigger : MonoBehaviour
{
    public SceneFader fader;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            fader.loadNextSceneByIndex = true;
            fader.FadeToScene();
        }
    }
}
