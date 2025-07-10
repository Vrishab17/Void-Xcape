using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class SceneLoaderTest
{
    private GameObject sceneLoaderObject;
    private SceneLoader sceneLoader;
    private bool sceneLoaded = false;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        sceneLoaderObject = new GameObject("SceneLoader");
        sceneLoader = sceneLoaderObject.AddComponent<SceneLoader>();
        sceneLoader.nextSceneName = "Level 1";

        // Register callback to detect scene load
        SceneManager.sceneLoaded += OnSceneLoaded;

        yield return null;
    }

    [UnityTest]
    public IEnumerator LoadNextScene_LoadsCorrectScene()
    {
        sceneLoader.LoadNextScene();

        // Wait until the sceneLoaded flag is true or timeout after 5 seconds
        float timeout = 5f;
        while (!sceneLoaded && timeout > 0f)
        {
            timeout -= Time.deltaTime;
            yield return null;
        }

        Assert.IsTrue(sceneLoaded, "Scene did not load.");
        Assert.AreEqual("Level 1", SceneManager.GetActiveScene().name);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Level 1")
            sceneLoaded = true;
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        // Clean up GameObject
        Object.Destroy(sceneLoaderObject);
        yield return null;
    }
}
