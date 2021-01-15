using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;


public class NewShield : MonoBehaviour
{
    public float knockbackForce = 40000.0f;
    public float damage = 25.0f;

    NewPlayer player;
    int durability;

    private void Start()
    {
        player = Singleton.instance.player;
        durability = Random.Range(3, 6);
    }

    private void FixedUpdate()
    {
        if (durability <= 0)
            Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == player.gameObject)
        {
            durability--;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Bullet bullet))
        {
            Projectile projectile = bullet.GetComponentInParent<Projectile>();

            durability--;
            projectile.fireRange += 2.0f;
            projectile.rb.velocity *= -5.0f;
        }
    }
}