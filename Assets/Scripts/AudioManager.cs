using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;
    public static AudioManager instance;
    private float musicVolume;

    // kaip naudoti
    // FindObjectOfType<AudioManager>().Play("pavad");
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        musicVolume = PlayerPrefs.GetFloat("MusicVolume");

       // DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = musicVolume * s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    private void Update()
    {
        if(PlayerPrefs.GetFloat("MusicVolume") != musicVolume)
        {
            musicVolume = PlayerPrefs.GetFloat("MusicVolume");
            foreach (Sound s in sounds)
            {
                s.source.volume = musicVolume * s.volume;
            }
        }
    }

    //void Start()
    //{
    //    Play("PlayerDeath");
    //}
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found");
            return;
        }
        s.source.Play();
    }
}
