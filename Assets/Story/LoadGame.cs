using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string nextSceneName = "GameScene"; // Replace with your scene name

    public void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
