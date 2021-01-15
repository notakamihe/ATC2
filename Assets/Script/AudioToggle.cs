using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class AudioToggle : MonoBehaviour
{
    public AudioMixer mixer;

    public TextMeshProUGUI musicText;
    public TextMeshProUGUI sfxText;

    // Start is called before the first frame update
    void Start()
    {
        mixer = Singleton.instance.mixer;
    }

    public void ToggleMusic ()
    {
        bool musicVolume = mixer.GetFloat("MusicVolume", out float vol);
        mixer.SetFloat("MusicVolume", vol == -80.0f ? 7.0f : -80.0f);
        musicText.color = vol == -80.0f ? Color.black : Color.grey; 
    }

    public void ToggleSFX ()
    {
        bool musicVolume = mixer.GetFloat("SFXVolume", out float vol);
        mixer.SetFloat("SFXVolume", vol == -80.0f ? 7.0f : -80.0f);
        sfxText.color = vol == -80.0f ? Color.black : Color.grey;
    }
}
