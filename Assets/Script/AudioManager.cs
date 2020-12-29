using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public string startMusicName;
    public bool startMusic;

    void Awake ()
    {
        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.name = sound.name;
            sound.source.clip = sound.clip;
            sound.source.loop = sound.loop;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.enabled = sound.enabled;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (startMusic)
        {
            StartCoroutine(Play(startMusicName, 0f));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Pause (string name, float delay = 0)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s != null && s.enabled)
        {
            yield return new WaitForSeconds(delay);
            s.source.Pause();
        }
    }

    public IEnumerator Pause (Sound sound, float delay = 0)
    {
        if (sound != null && sound.enabled)
        {
            yield return new WaitForSeconds(delay);
            sound.source.Pause();
        }
    }

    public IEnumerator Play (string name, float delay = 0)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s != null && s.enabled)
        {
            yield return new WaitForSeconds(delay);
            s.source.Play();
        }
    }

    public IEnumerator Play (Sound sound, float delay = 0)
    {
        if (sound != null && sound.enabled)
        {
            yield return new WaitForSeconds(delay);
            sound.source.Play();
        }
    }

    public void PlayButtonClick ()
    {
        Sound buttonClick = GetSound("ButtonClick");

        if (buttonClick != null && buttonClick.enabled)
        {
            buttonClick.source.Play();
        }
    }

    public IEnumerator Stop (string name, float delay = 0)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s != null && s.enabled)
        {
            yield return new WaitForSeconds(delay);
            s.source.Stop();
        }
    }

    public IEnumerator Stop (Sound sound, float delay = 0)
    {
        if (sound != null && sound.enabled)
        {
            yield return new WaitForSeconds(delay);
            sound.source.Stop();
        }
    }

    public IEnumerator UnPause (string name, float delay = 0)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s != null && s.enabled)
        {
            yield return new WaitForSeconds(delay);
            s.source.UnPause();
        }
    }

    public IEnumerator UnPause (Sound sound, float delay = 0)
    {
        if (sound != null && sound.enabled)
        {
            yield return new WaitForSeconds(delay);
            sound.source.UnPause();
        }
    }

    public Sound GetSound (string name)
    {
        return Array.Find(sounds, sound => sound.name == name);
    }
}
