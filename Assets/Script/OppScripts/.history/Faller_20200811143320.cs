using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Faller : MonoBehaviour
{
    public Opp opp;
    public GameObject firePt;
    public GameObject projectile;
    public GameObject gun;
    public float stopRange = 1;
    public float maxPlyrSpdForFall = 13f;
    public float detectRangeShoot;
    public float bulletSpd = 10000;
    public float fireRate = 1.5f;
    public float fireRange = .5f;
    public float floatHeight = 3;
    public bool fell = false;
    public bool grounded = false;
    public bool cooledDown = true;

    // Start is called before the first frame update
    void Start()
    {
        detectRangeShoot = opp.detectionRadius / 3 * 2;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!opp.isDead)
        {
            FollowPlayer();
            FixAltitude();
        } else
        {
            Fall();
        }
    }

    void OnCollisionEnter (Collision col)
    {
        if (col.gameObject.tag == "Ground")
        {
            grounded = true;
        }
    }

    void FollowPlayer ()
    {
        if (opp.DistFrmPlayer() <= opp.detectionRadius)
        {
            opp.FixRotToPlayer();

            if (!fell)
            {
                float distFrmPlayerHori = Mathf.Sqrt(
                    Mathf.Pow(opp.player.transform.position.x - transform.position.x, 2) + 
                    Mathf.Pow(opp.player.transform.position.z - transform.position.z, 2)
                );

                if (distFrmPlayerHori >= stopRange)
                {
                    opp.rb.AddRelativeForce(0, 0, opp.speed * Time.deltaTime);
                } else
                {
                    if (opp.player.rb.velocity.magnitude < maxPlyrSpdForFall)
                    {
                        Debug.Log("FALL");
                        fell = true;
                        Fall();
                    }
                }
            }

            if (grounded)
            {
                if (opp.DistFrmPlayer() <= detectRangeShoot)
                {
                    Shoot();
                }
            }
        }
    }

    void Shoot ()
    {
        if (cooledDown)
        {
            cooledDown = false;
            FireBullet();
            StartCoroutine(ShootCoolDown(fireRate));
        }

    }

    void FireBullet ()
    {
        GameObject bullet = Instantiate(projectile, firePt.transform.position, transform.rotation);
        gun.GetComponent<AudioSource>().Play();

        for (int i = 0; i < 10; i++)
        {
            bullet.GetComponent<Rigidbody>().AddRelativeForce(0, 0, bulletSpd * Time.deltaTime);
            bullet.GetComponent<Rigidbody>().AddForce(0, .2f, 0);
        }

        Destroy(bullet, fireRange);
    }

    void Fall ()
    {
        opp.rb.drag = 0;
        opp.rb.useGravity = true;
    }

    IEnumerator ShootCoolDown (float delay)
    {
        yield return new WaitForSeconds(delay);
        cooledDown = true;
    }

    void FixAltitude ()
    {
        if (!fell)
        {
            transform.position = new Vector3(transform.position.x, floatHeight, transform.position.z);
        }
    }
}
