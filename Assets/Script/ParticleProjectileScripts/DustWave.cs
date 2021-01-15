using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DustWave : MonoBehaviour
{
    public ParticleSystem partSys;
    public ParticleSystem.Particle[] particles;
    public NewPlayer player;

    public float damage = .3f;
    public float blowForce = 10000f;

    // Start is called before the first frame update
    void Start()
    {
        player = Singleton.instance.player;
        particles = new ParticleSystem.Particle[partSys.main.maxParticles];
    }

    // Update is called once per frame
    void Update()
    {
        int numAliveParticles = partSys.GetParticles(particles);

        for (int i = 0; i < numAliveParticles; i++)
        {
            if (Vector3.Distance(Quaternion.AngleAxis(90, Vector3.right) * particles[i].position, 
                player.transform.position) <= 5.0f)
            {
                print("Player near");
                player.Jump(750.0f);
                break;
            }
        }

        partSys.SetParticles(particles, numAliveParticles);

        if (!partSys.isPlaying)
        {
            Destroy(this.gameObject);
        }
    }
}
