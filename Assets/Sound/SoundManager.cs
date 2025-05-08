using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    //private Slider volumeSlider;  // Declare the Slider variable

    //void Start()
    //{
        // Find the Slider in the "Settings" panel under "MainMenu"
        //volumeSlider = GameObject.Find("MainMenu/Settings/Slider").GetComponent<Slider>();

        // Set the min and max values for the volume slider
        //volumeSlider.minValue = -80f;  // Example min value (dB)
        //volumeSlider.maxValue = 0f;    // Example max value (dB)

        // Optionally, you can set an initial value for the slider
        //volumeSlider.value = 0f;  // Set default volume to 0 dB (full volume)

        // Add a listener to update the volume when the slider value changes
        //volumeSlider.onValueChanged.AddListener(SetVolume);
    //}
}
