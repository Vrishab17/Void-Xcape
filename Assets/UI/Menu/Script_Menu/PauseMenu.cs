using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio; // Add this for AudioMixer
public class PauseMenu : MonoBehaviour
{
    [Header("Pause Menu")]
    public GameObject pauseMenuUI;
    public KeyCode pauseKey = KeyCode.P;
    public KeyCode alternatePauseKey = KeyCode.Escape;
   
    [Header("Audio Settings")]
    public Slider volumeSlider; // Assign in Inspector
    public AudioMixer MainAudioMixer; // Assign in Inspector
   
    public static bool GameIsPaused = false;
   
    void Start()
    {
        // Audio setup
        if (volumeSlider != null && MainAudioMixer != null)
        {
            // Set min and max values for volume in decibels
            volumeSlider.minValue = -80f;
            volumeSlider.maxValue = 20f;
           
            // Load saved volume (default to 0 dB) and apply to slider + mixer
            float savedVolume = PlayerPrefs.GetFloat("volume", 0f);
            volumeSlider.value = savedVolume;
            MainAudioMixer.SetFloat("Volume", savedVolume);
           
            // Update volume live on slider change
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
       
        // Pause menu setup
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);
           
        GameIsPaused = false;
        Time.timeScale = 1f;
    }
   
    void Update()
    {
        if (Input.GetKeyDown(pauseKey) || Input.GetKeyDown(alternatePauseKey))
        {
            if (GameIsPaused)
                Resume();
            else
                Pause();
        }
    }
   
    // Add the SetVolume method
    public void SetVolume(float volume)
    {
        MainAudioMixer.SetFloat("Volume", volume);
        PlayerPrefs.SetFloat("volume", volume);
        PlayerPrefs.Save();
    }
   
    public void Resume()
    {
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);
           
        Time.timeScale = 1f;
        GameIsPaused = false;
       
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
   
    public void Pause()
    {
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(true);
           
        Time.timeScale = 0f;
        GameIsPaused = true;
       
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
   
public void MainMenu()
{
    Time.timeScale = 1f;
    GameIsPaused = false;
    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true;

    string currentSceneName = SceneManager.GetActiveScene().name;
    PlayerPrefs.SetString("LastPlayedScene", currentSceneName);
    PlayerPrefs.SetInt("ReturnedToMenu", 1);
    PlayerPrefs.Save();

    Debug.Log("Saving scene name: " + currentSceneName);
    Debug.Log("Set ReturnedToMenu to 1");

    StartCoroutine(LoadMenuAfterDelay());
}

private IEnumerator LoadMenuAfterDelay()
{
    yield return new WaitForEndOfFrame(); // optional: wait to ensure save
    SceneManager.LoadScene("Menu");
}
   
    public void QuitGame()
    {
        Time.timeScale = 1f;
       
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}