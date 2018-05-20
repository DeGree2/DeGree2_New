using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour {

    
	public AudioSource music;
	public AudioMixer audioMixer;
	public Slider musicVolume;
	public Slider fxVolume;

	void Start()
	{
        musicVolume.value = PlayerPrefs.GetFloat("MusicVolume");
        fxVolume.value = PlayerPrefs.GetFloat("FXVolume");
	}

	void Update()
	{
        music.volume = musicVolume.value;
        Debug.Log(musicVolume.value);
	}

    public void VolumePrefs()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicVolume.value);
        PlayerPrefs.SetFloat("FXVolume", fxVolume.value);
    }
}
