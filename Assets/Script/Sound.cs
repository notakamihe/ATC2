using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]

public class Sound
{
   
    public AudioClip clip;
    [HideInInspector] public AudioSource source;
    [Range(0f, 1f)] public float volume;
    [Range(0f, 1f)] public float pitch;
    [Range(0f, 1f)] public float spatialBlend;
    public string name;
    public bool loop;
    public bool enabled;
}
