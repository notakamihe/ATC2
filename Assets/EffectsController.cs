using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsController : MonoBehaviour
{
    public AudioSource[] sfxs; 
    public AudioManager audioMgr;
    public GameControl gameCtrlr;
    public bool sfxOn = true;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (audioMgr == null)
        {
            audioMgr = gameCtrlr.audioManager;
        }

        sfxs = FindObjectsOfType<AudioSource>();

        if (!sfxOn)
        {
            ToggleSFX(true);
        } else
        {
            ToggleSFX(false);
        }

        
    }

    void ToggleSFX (bool enabled)
    {
        foreach (AudioSource source in sfxs)
        {
            if (!Array.Exists(audioMgr.sounds, sound => sound.source.clip == source.clip))
            {
                source.mute = enabled;
            }
        }
    }

    public void ToggleSFXOn ()
    {
        sfxOn = !sfxOn;
    }
}
