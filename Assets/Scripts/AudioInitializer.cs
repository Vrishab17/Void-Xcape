using UnityEngine;
using UnityEngine.Audio;

public class AudioInitializer : MonoBehaviour
{
    public AudioMixer NewAudioMixer;

    void Start()
    {
        if (NewAudioMixer != null)
        {
            // Check if volume is saved in PlayerPrefs, otherwise set a default volume
            if (PlayerPrefs.HasKey("volume"))
            {
                float savedVolume = PlayerPrefs.GetFloat("volume");
                NewAudioMixer.SetFloat("Volume", savedVolume);
                Debug.Log("Applied saved volume: " + savedVolume);
            }
            else
            {
                // Apply a default volume if nothing is saved
                NewAudioMixer.SetFloat("Volume", 0f);
                Debug.Log("No saved volume, applying default (0 dB)");
            }
        }
        else
        {
            Debug.LogWarning("AudioMixer is not assigned in AudioInitializer!");
        }
    }
}