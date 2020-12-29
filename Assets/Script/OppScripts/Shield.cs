using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public Player player;
    public GameObject[] projectiles;
    public float knockbackForce = 7000;
    public float knockbackForceScale = 8f;
    public float damage = .3f;
    public float reflectSpd = 30000f;
    public int durability;
    public bool bulletDeflected = false;

    // Start is called before the first frame update
    void Start()
    {
        durability = Random.Range(3, 6);
    }

    // Update is called once per frame
    void Update()
    {
        player = transform.parent.gameObject.GetComponent<Opp>().player;
        GetProjectiles();
        CheckDurability();
    }

    void OnCollisionEnter (Collision col)
    {
        if (col.gameObject == player.gameObject)
        {
            durability--;
        }
    }

    void CheckDurability ()
    {
        if (durability <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    void GetProjectiles ()
    {
        if (!bulletDeflected)
        {
            projectiles = GameObject.FindGameObjectsWithTag("Projectiles");

            foreach (GameObject projectile in projectiles)
            {
                Projectile proj = projectile.GetComponent<Projectile>();

                if (Mathf.Abs((transform.position + transform.forward * 1).z - proj.transform.position.z) <= 1 &&
                    Mathf.Abs((transform.position + transform.forward * 1).x - proj.transform.position.x) <= 1.3)
                {
                    DeflectBullet(proj.gameObject);
                    bulletDeflected = true;
                    durability--;
                    StartCoroutine(ReenableDeflect(.1f));
                }
            }
        }
    }

    IEnumerator ReenableDeflect (float delay)
    {
        yield return new WaitForSeconds(delay);
        bulletDeflected = false;
    }

    void DeflectBullet (GameObject proj)
    {
        Projectile projectile = proj.GetComponent<Projectile>();
        projectile.deflected = true;
        projectile.rb.AddRelativeForce(0, 0, -reflectSpd);
    }
}
