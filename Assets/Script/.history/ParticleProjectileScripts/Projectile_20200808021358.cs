using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public static float shootTime;
    public GameObject shooter;
    public Rigidbody rb;
    public float fireRange;
    public bool hitSomething = false;
    public bool isProjectileExplosive = false;
    public bool shot = false;
    public bool deflected = false;
    public GameObject explosion;

    // Start is called before the first frame update
    void Start()
    {
        shootTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (shot)
        {
            CheckDestroy();
        }

        if (deflected)
        {
            fireRange *= 3;

            if (Random.Range(0, 15) == 0)
            {
                rb.MovePosition(shooter.transform.position);
            }
            
            deflected = false;
        }
    }

    void OnCollisionEnter (Collision col)
    {
        if (col.collider.tag != "OppShield")
        {
            hitSomething = true;
        }
    }

    public void CheckDestroy ()
    {
        if (Time.time - shootTime < fireRange)
        {


            if (hitSomething)
            {
                DestroyProjectile();
            }
        } else
        {
            DestroyProjectile();
        }
    }

    void DestroyProjectile ()
    {
        if (isProjectileExplosive)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
        }

        Destroy(this.gameObject);
    }
}