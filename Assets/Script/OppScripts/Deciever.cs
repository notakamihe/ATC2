using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Deciever : MonoBehaviour
{
    public AudioSource inflate;
    public Opp opp;
    public Vector3 growLimit = new Vector3(2, 2, 2);
    public float growRange = 4f;
    public float growthSpd = .2f;

    // Start is called before the first frame update
    void Start()
    {
        inflate = GetComponents<AudioSource>()[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (!opp.isDead)
        {
            if (!opp.player.isDead)
            {
                FollowPlayer();
            }
        }
    }

    void FollowPlayer()
    {
        if (Vector3.Distance(transform.position, opp.player.transform.position) <= opp.detectionRadius)
        {
            opp.rb.constraints = RigidbodyConstraints.FreezeRotationX;

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(opp.player.transform.position - transform.position),
                opp.rotSpeed * Time.deltaTime
            );
            
            opp.rb.AddRelativeForce(0, 0, opp.speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, opp.player.transform.position) <= growRange)
            {
                Grow();
            } else
            {
                if (inflate.isPlaying)
                {
                    inflate.Stop();
                }
            }
        } else
        {
            opp.rb.constraints &= ~RigidbodyConstraints.FreezeRotationX;
        }
    }

    void Grow ()
    {
        if (transform.localScale.x <= growLimit.x && 
            transform.localScale.y <= growLimit.y && 
            transform.localScale.z <= growLimit.z)
        {
            transform.localScale += new Vector3(growthSpd, growthSpd, growthSpd);
            opp.rb.mass *= 1.25f;
            opp.speed *= 1.3f;
            opp.health *= 1.25f;
            opp.defenseMod *= .9f;
            opp.damage *= 1.2f;
            opp.jumpForce *= 1.25f;

            if (!inflate.isPlaying)
            {
                inflate.Play();
            }
        } else
        {
            if (inflate.isPlaying)
            {
                inflate.Stop();
            }
        }
    }
}
