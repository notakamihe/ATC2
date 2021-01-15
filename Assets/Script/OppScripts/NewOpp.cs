using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class NewOpp : Character
{
    public NewPlayer player;

    public float playerDetectionRadius = 15.0f;
    public float knockbackForce = 20000.0f;

    public float DistanceFromPlayer
    {
        get
        {
            return Vector3.Distance(player.transform.position, transform.position);
        }
    }

    public Vector3 DirToPlayer
    {
        get
        {
            return player.transform.position - transform.position;
        }
    }
    protected bool IsNearEdges
    {
        get
        {
            return !Singleton.instance.areaNotNearEdges.bounds.Contains(transform.position);
        }
    }

    protected void Start()
    {
        base.Start();

        player = Singleton.instance.player;
    }

    protected void FixedUpdate()
    {
        base.Update();

        if (isDead)
        {
            Die();
        }

        if (PlayerInRange(playerDetectionRadius) && !player.isDead)
        {
            Look(player.transform.position);
            Approach();
        }
    }

    protected virtual void Approach ()
    {
        Move();
    }

    protected override void Die()
    {
        if (shouldFlip)
        {
            Flip();
            transform.localEulerAngles = new Vector3(-270.0f, transform.localEulerAngles.y, 0);
        }

        Destroy(this.gameObject, 3.0f);
        Destroy(this);
    }

    protected void Look (Vector3 at)
    {
        Vector3 direction = at - transform.position;
        direction.y = 0.0f;

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(direction),
            rotSpeed * Time.deltaTime
        );
    }

    protected void Move()
    {
        rb.AddRelativeForce(Vector3.forward * speed * Time.deltaTime);
    }

    protected void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        if (collision.collider.TryGetComponent(out Bullet bullet))
        {
            TakeDamage(bullet.damage * player.bulletDmgMod * defenseModifier);

            if (!PlayerInRange(playerDetectionRadius))
                StartCoroutine(Search(player.transform.position));
        }

        if (collision.collider.TryGetComponent(out NewPlayer plyr) && plyr.isInvincible)
        {
            TakeDamage(plyr.meleeDamage * defenseModifier);
        }
    }

    protected void OnParticleCollision(GameObject other)
    {
        if (other.TryGetComponent(out Explosion explosion))
        {
            TakeDamage(TryGetComponent(out NewBoss boss) ? explosion.damage * 0.2f : explosion.damage);
        }
    }

    protected bool PlayerInRange (float range)
    {
        return DistanceFromPlayer <= range;
    }

    IEnumerator Search (Vector3 target)
    {
        float searchTime = Time.time;

        while (Time.time < searchTime + 4.0f)
        {
            if (PlayerInRange(playerDetectionRadius) || IsNearEdges)
                break;

            Look(target);
            Move();
            yield return null;
        }
    }
}