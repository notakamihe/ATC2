using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Opp : MonoBehaviour
{
    public GameControl gameCtrlr;
    public Player player;
    public Rigidbody rb;
    public Image healthBar;
    public Animation flip;
    public GameObject[] bullets;
    public GameObject spawner;
    public float speed = 1000f;
    public float rotSpeed = 5f;
    public float detectionRadius = 20f;
    public float health = 1f;
    public float knockbackForce = 30000f;
    public float knockbackForceScale = 8f;
    public float damage = .1f;
    public float defenseMod = 1f;
    public float aimAssistRange = 3f;
    public float flipForce = 5000f;
    public float spawnY = 0;
    public float jumpDetectRng = 3f;
    public float jumpForce = 500;
    public bool isDead = false;
    public bool shouldFlip = true;
    public bool removed = false;
    public bool restrictRoll = true;
    public bool relevant = true;
    public bool grounded;
    public bool canJump = true;
    
    void Start ()
    {
        gameCtrlr = FindObjectOfType<GameControl>();
        player = FindObjectOfType<Player>();
        rb = GetComponent<Rigidbody>();
        flip = GetComponent<Animation>();
    }

    void Update ()
    {
        if (!isDead)
        {
            CheckHealthDepleted();
            KillIfOffMap();
            ScaleHealthBar();
            Straighten();
            CheckObstacles();
            AttractBullets();
        } else
        {
            if (!removed)
            {
                removed = true;

                if (shouldFlip)
                {
                    Flip();
                }
                
                if (!gameCtrlr.bossLevel && relevant)
                {
                    gameCtrlr.oppCountSimult--;
                    gameCtrlr.oppsDead++;
                }

                if (spawner != null)
                {
                    if (spawner.GetComponent<Boss>() != null)
                    {
                        spawner.GetComponent<Boss>().oppCount--;
                    }
                }

                if (GetComponent<Boss>() != null)
                {
                    gameCtrlr.bossCountDead++;
                }
            }

            Destroy(this.gameObject, 2f);
        }
    }

    void OnCollisionEnter (Collision col)
    {
        if (col.collider.tag == "Bullets")
        {
            TakeDamage(col.collider.gameObject.GetComponent<Bullet>().damage * player.bulletDmgMod * defenseMod);
        }

        if (col.gameObject == player.gameObject)
        {
            if (player.isInvincible)
            {
                TakeDamage(player.meleeDamage * defenseMod);
            }
        }

        if (col.gameObject.tag == "Ground")
        {
            grounded = true;
        }
    }

    void OnParticleCollision (GameObject obj)
    {
        if (obj.tag == "BulletExplosion")
        {
            if (GetComponent<Boss>() != null)
            {
                TakeDamage(obj.GetComponent<Explosion>().damage * 0.02f);
            } else
            {
                TakeDamage(obj.GetComponent<Explosion>().damage);
            }
        }
    }

    void AttractBullets ()
    {
        bullets = GameObject.FindGameObjectsWithTag("Projectiles");

        foreach (GameObject bullet in bullets)
        {
            if (Vector3.Distance(transform.position, bullet.transform.position) <= aimAssistRange)
            {
                bullet.transform.position = Vector3.MoveTowards(bullet.transform.position, transform.position, 50 * Time.deltaTime);
            }
        }
    }

    void CheckHealthDepleted ()
    {
        if (health <= 0)
        {
            health = 0;
            isDead = true;
        }
    }

    void CheckObstacles ()
    {
        if (grounded && canJump)
        {
            Collider[] surroundings = Physics.OverlapSphere(transform.position, jumpDetectRng);

            foreach (Collider obj in surroundings)
            {
                Vector3 relativePtOpp = transform.InverseTransformPoint(obj.gameObject.transform.position);
                Vector3 relativePtObs = transform.InverseTransformPoint(player.transform.position);
                bool playerinRange = DistFrmPlayer() <= detectionRadius && player.jumped;
                bool isObstacle = obj.gameObject.GetComponent<Obstacle>() != null;
                bool oppTaller = obj.gameObject.transform.localScale.y - transform.localScale.y <= transform.localScale.y * 1.5;

                if (relativePtOpp.z > 0 && 
                    isObstacle && 
                    oppTaller &&
                    (playerinRange && Mathf.Abs(relativePtObs.x) <= (obj.gameObject.transform.localScale.x/2) - 1))
                {
                    print("OBSTACLE INCOMING");
                    Jump();
                }
            }
        }
    }

    public float DistFrmPlayer ()
    {
        return Vector3.Distance(player.transform.position, transform.position);
    }

    public Quaternion FixRotToPlayer ()
    {
        Quaternion rotToPlayer = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(player.transform.position - transform.position),
            rotSpeed * Time.deltaTime
        );

        transform.rotation = rotToPlayer;
        return transform.rotation;
    }

    void Flip ()
    {
        for (int i = 0; i < 10; i++)
        {
            rb.AddForce(0, flipForce * Time.deltaTime, 0);
            rb.AddRelativeForce(0, 0, -3000 * Time.deltaTime);
        }

        flip.Play();
    }

    void Jump()
    {
        grounded = false;
        rb.AddForce(0, jumpForce, 0);
    }

    void KillIfOffMap ()
    {
        if (transform.position.y <= -3)
        {
            shouldFlip = false;
            isDead = true;
        }
    }

    void ScaleHealthBar ()
    {
        healthBar.transform.localScale = new Vector3(
            health, 
            healthBar.transform.localScale.y, 
            healthBar.transform.localScale.z
        );
    }

    void Straighten ()
    {
        if (restrictRoll)
        {
            Quaternion rotation = transform.rotation;
            rotation[0] = 0;
            rotation[2] = 0;
            transform.rotation = rotation;
        }
    }

    void TakeDamage (float damage)
    {
        if (damage > health)
        {
            health = 0;
        } else
        {
            health -= damage;
        }
    }
}
