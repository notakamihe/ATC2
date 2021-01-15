using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;


public class Singleton : MonoBehaviour
{
    public static Singleton instance;

    void Awake()
    {
        instance = this;
        instance.level = FindObjectOfType<Level>();
        instance.audioManager = FindObjectOfType<AudioManager>();
    }

    public Level level;
    public AudioManager audioManager;
    public NewPlayer player;
    public BoxCollider areaNotNearEdges;
    public AudioMixer mixer;
}
