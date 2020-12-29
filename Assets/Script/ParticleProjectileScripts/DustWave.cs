using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustWave : MonoBehaviour
{
    public ParticleSystem partSys;
    public float damage = .3f;
    public float blowForce = 10000f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!partSys.isPlaying)
        {
            Destroy(this.gameObject);
        }
    }
}
