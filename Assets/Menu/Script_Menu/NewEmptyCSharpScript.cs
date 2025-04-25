using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public AudioMixer NewAudioMixer;
    private Slider volumeSlider;  // Declare the Slider variable

    void Start()
    {
        // Find the Slider in the "Settings" panel under "MainMenu"
        volumeSlider = GameObject.Find("MainMenu/Settings/Volume").GetComponent<Slider>();

        // Set the min and max values for the volume slider
        volumeSlider.minValue = -80f;  // Example min value (dB)
        volumeSlider.maxValue = 0f;    // Example max value (dB)

        // Optionally, you can set an initial value for the slider
        volumeSlider.value = 0f;  // Set default volume to 0 dB (full volume)

        // Add a listener to update the volume when the slider value changes
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    // Load Scene
    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Debug.Log("The Player Test 1");
    }

    // Quit Game
    public void Quit()
    {
        Application.Quit();
        Debug.Log("The Player has Quit the game");
    }

    // Quality
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    // Volume
    public void SetVolume(float volume)
    {
        // Adjust the volume in the AudioMixer
        NewAudioMixer.SetFloat("Volume", volume);
    }
}