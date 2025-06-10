using UnityEngine;

public class LevelEndTrigger : MonoBehaviour
{
    public SceneFader fader;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //complete final objective
            ObjectiveManager mgr = FindFirstObjectByType<ObjectiveManager>();
            if (mgr != null && mgr.GetCurrentIndex() == 4) 
            {
                mgr.CompleteCurrentObjective();
            }

            // Proceed to next scene
            fader.loadNextSceneByIndex = true;
            fader.FadeToScene();
        }
    }
}
