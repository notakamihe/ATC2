using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonMist : MonoBehaviour
{
    public ParticleSystem partSys;
    public float damage = .03f;

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
