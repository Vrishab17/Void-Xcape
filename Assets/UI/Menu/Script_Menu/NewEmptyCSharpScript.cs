using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public AudioMixer MainAudioMixer;
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
    }

    // Called when the "Play" button is pressed
    public void Play()
    {
        // Reapply saved volume to AudioMixer before scene load
        float savedVolume = PlayerPrefs.GetFloat("volume", 0f);
        MainAudioMixer.SetFloat("Volume", savedVolume);

        // Load the next scene in the build index
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Debug.Log("Loading next scene with saved volume: " + savedVolume + " dB");
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
}
