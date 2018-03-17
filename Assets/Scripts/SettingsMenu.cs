using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour {

    public AudioMixer audioMixer;

    public void setVolume(float slider_input)
    {
        audioMixer.SetFloat("volume", slider_input);
    }
}
