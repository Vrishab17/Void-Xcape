using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public AudioMixer MainAudioMixer;
    public TextMeshProUGUI playButtonText; // Assign your TextMeshPro UI component in the inspector
    
    private Slider volumeSlider;
    
    void Start()
    {
        // Find the Slider in the "Settings" panel under "MainMenu"
        volumeSlider = GameObject.Find("MainMenu/Settings/Slider").GetComponent<Slider>();
        // Set min and max values for volume in decibels
        volumeSlider.minValue = -80f;
        volumeSlider.maxValue = 0f;
        // Load saved volume (default to 0 dB) and apply to slider + mixer
        float savedVolume = PlayerPrefs.GetFloat("volume", 0f);
        volumeSlider.value = savedVolume;
        MainAudioMixer.SetFloat("Volume", savedVolume);
        // Update volume live on slider change
        volumeSlider.onValueChanged.AddListener(SetVolume);
        
        // Update the play button text based on game state BEFORE resetting the flag
        UpdatePlayButtonText();
        
    }
    
    // Update the button text based on whether the player has started a game
    private void UpdatePlayButtonText()
    {
        if (playButtonText != null)
        {
            // Check if player has started a game AND returned to menu (using PlayerPrefs as flags)
            bool hasStartedGame = PlayerPrefs.GetInt("HasStartedGame", 0) == 1;
            bool hasReturnedToMenu = PlayerPrefs.GetInt("ReturnedToMenu", 0) == 1;
            
            if (hasStartedGame && hasReturnedToMenu)
            {
                playButtonText.text = "Continue";
            }
            else
            {
                playButtonText.text = "Start";
            }
        }
    }

    // Called when the "Play" button is pressed
    public void Play()
    {
        // Reapply saved volume to AudioMixer before scene load
        float savedVolume = PlayerPrefs.GetFloat("volume", 0f);
        MainAudioMixer.SetFloat("Volume", savedVolume);

        // Check if this is "Start" or "Continue"
        bool hasStartedGame = PlayerPrefs.GetInt("HasStartedGame", 0) == 1;
        bool hasReturnedToMenu = PlayerPrefs.GetInt("ReturnedToMenu", 0) == 1;
        string savedScene = PlayerPrefs.GetString("LastPlayedScene", "Prolog");

        // Debug logging
        Debug.Log("HasStartedGame: " + hasStartedGame);
        Debug.Log("HasReturnedToMenu: " + hasReturnedToMenu);
        Debug.Log("SavedScene: " + savedScene);

        if (hasStartedGame && hasReturnedToMenu)
        {
            // Continue - load the saved scene
            SceneManager.LoadScene(savedScene);
            Debug.Log("Continuing from saved scene: " + savedScene);
        }
        else
        {
            // Start - load the prolog scene and mark game as started
            PlayerPrefs.SetInt("HasStartedGame", 1);
            PlayerPrefs.Save();
            SceneManager.LoadScene("Prolog");
            Debug.Log("Starting new game - Loading Prolog");
        }
        
        if (hasStartedGame && hasReturnedToMenu)
        {
            SceneManager.LoadScene(savedScene);
            Debug.Log("Continuing from saved scene: " + savedScene);
        }
        else
        {
            PlayerPrefs.SetInt("HasStartedGame", 1);
            PlayerPrefs.Save();
            SceneManager.LoadScene("Prolog");
            Debug.Log("Starting new game - Loading Prolog");
        }

        // Reset flag AFTER you've decided what to load
        PlayerPrefs.SetInt("ReturnedToMenu", 0);
        PlayerPrefs.Save();

    }   
    
    // Called when the "Quit" button is pressed
    public void Quit()
    {
        Application.Quit();
        Debug.Log("The Player has Quit the game");
    }
    
    // Set the current quality level
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
    
    // Called when volume slider is changed
    public void SetVolume(float volume)
    {
        MainAudioMixer.SetFloat("Volume", volume);
        PlayerPrefs.SetFloat("volume", volume);
    }
    
    // Optional: Call this method if you want to reset the game state (for testing or new game)
    public void ResetGameState()
    {
        PlayerPrefs.DeleteKey("HasStartedGame");
        UpdatePlayButtonText();
        Debug.Log("Game state reset - button will show 'Start' again");
    }
}