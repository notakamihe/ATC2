using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public Opp opp;
    public GameObject projectile;
    public GameObject firePt;
    public GameObject gun;
    public float detectRadiusMelee = 10f;
    public float fireRate = .8f;
    public float fireRange = 1f;
    public float bulletSpd = 10000f;
    public bool projCooledDown = false;

    void Start ()
    {

    }

    void Update ()
    {
        if (!opp.isDead && !opp.player.justSpawned)
        {
            FollowPlayer();
        }
    }

    void FollowPlayer ()
    {
        if (Vector3.Distance(opp.player.transform.position, transform.position) <= opp.detectionRadius)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(opp.player.transform.position - transform.position),
                opp.rotSpeed * Time.deltaTime
            );

            if (Vector3.Distance(opp.player.transform.position, transform.position) <= detectRadiusMelee)
            {
                opp.rb.AddRelativeForce(0, 0, opp.speed * Time.deltaTime);
            } else
            {
                Shoot();
            }
        }
    }

    void Shoot ()
    {
        if (!projCooledDown)
        {
            projCooledDown = true;
            FireBullet();
            StartCoroutine(CoolDown(fireRate));
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

    IEnumerator CoolDown (float delay)
    {
        yield return new WaitForSeconds(delay);
        projCooledDown = false;
    }

    void OnDrawGizmosSelected ()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, opp.detectionRadius);
        Gizmos.DrawWireSphere(transform.position, detectRadiusMelee);
    }
}
