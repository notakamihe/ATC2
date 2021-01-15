using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float damage = .5f;
    public ParticleSystem particles;

    void Start ()
    {
        Destroy(this.gameObject, particles.main.duration);
    }
}
