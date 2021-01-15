using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class NewShooter : NewOpp, IShooter, IJumper
{
    public GameObject gun;
    public Projectile ammo;

    public float fireRange = 1.0f;
    public float fireRate = 0.8f;
    public float rangedDamage = 25.0f;
    public float meleeRange = 10.0f;

    Vector3 firePoint;
    float lastShot = 0.0f;

    protected void Start()
    {
        base.Start();
    }

    protected void FixedUpdate()
    {
        base.FixedUpdate();

        firePoint = gun.transform.position + gun.transform.forward * 1.0f;
        DetectForJump();
    }

    protected override void Approach()
    {
        if (PlayerInRange(meleeDamage))
        {
            Move();
        }
        else
        {
            Shoot();
        }
    }

    public virtual void DetectForJump()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 8.0f, LayerMask.GetMask("Obstacle")))
        {
            if (Vector3.Angle(transform.forward, rb.velocity) <= 25 && rb.velocity.magnitude > 5.0f)
            {
                Jump(jumpForce);
            }
        }
    }

    public void FireShot(Vector3 direction)
    {
        Projectile projectile = Instantiate(ammo, firePoint, transform.rotation);

        gun.GetComponent<AudioSource>().Play();

        projectile.isShot = true;
        projectile.shooter = this.gameObject;
        projectile.fireRange = fireRange;

        for (int i = 0; i < 15; i++)
            projectile.GetComponent<Rigidbody>().AddForce(
                (direction * 5000.0f + Vector3.up * 50) * Time.deltaTime);
    }

    public virtual void Jump(float force)
    {
        if (grounded)
        {
            rb.AddForce(0, force * Time.deltaTime, 0, ForceMode.VelocityChange);
        }
    }

    public void Shoot()
    {
        if (Time.time > lastShot + fireRate)
        {
            lastShot = Time.time;
            FireShot(transform.forward);
        }
    }
}