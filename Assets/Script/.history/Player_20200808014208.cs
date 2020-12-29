using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Rigidbody rb;
    public Animation flip;
    public FollowPlayer cameraMain;
    public Hearts hearts;
    public Image healthBar;
    public GameControl gameCtrlr;
    public GameObject firePoint;
    public GameObject projectile;
    public GameObject bulletExplosion;
    public ParticleSystem particles;
    public Quaternion rotation;
    public float justSpawnedDuration = 3f;
    public float health = 1f;
    public float speed = 2000;
    public float rotSpeed = 5;
    public float jumpForce = 5000;
    public float firstTapTime;
    public float cooldown;
    public float fireRate = 1f;
    public float fireRange = 0.5f;
    public float bulletDmgMod = 1f;
    public float meleeDamage = .2f;
    public float poisonDuration = 10f;
    public float poisonInterval = 1f;
    public float spawnSafeZone = 5f;
    public float dashCooldown = 3f;
    public float tapInterval = 0.15f;
    public int lives = 3;
    public int tapCount = 1;
    public bool justSpawned = true;
    public bool grounded = true;
    public bool isCooling = true;
    public bool isDead = false;
    public bool canRoll = false; 
    public bool shouldFlip = true;
    public bool deathFinished = false;
    public bool canMove = true;
    public bool bulletEnabled = true;
    public bool tripleMode = false;
    public bool isInvincible = false;
    public bool isProjectileExplosive = false;
    public bool poisoned = false;
    public bool jumped = false;
    public bool dashEnabled = true;

    // Start is called before the first frame update
    void Start ()
    {
        gameCtrlr = FindObjectOfType<GameControl>().GetComponent<GameControl>();
        cooldown = 3f;
        rotation = transform.rotation;
        hearts.heartCount = lives;
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        if (isDead && !deathFinished)
        {
            AfterDeath();
        }

        if (canMove)
        {
            ToggleJustSpawned();
            KillIfOffMap();
            Move();
            RotateLateralMvt();
            Jump();
            ListenForDash();
            Straighten();
            Shoot();
        }

        healthBar.transform.localScale = new Vector3(health, transform.localScale.y, transform.localScale.z);
        transform.rotation = rotation;
    }

    
    void OnCollisionEnter (Collision clldr)
    {
        if (clldr.collider.tag == "Ground")
        {
            grounded = true;
            jumped = false;
        }

        if (!isDead)
        {
            if (clldr.collider.tag == "Opps")
            {
                Opp opp = clldr.gameObject.GetComponent<Opp>();

                if (!opp.isDead)
                {
                    Knockback(opp.transform.position, opp.knockbackForce, opp.knockbackForceScale);

                    if (!isInvincible)
                    {
                        TakeDamage(opp.damage);

                        if (opp.gameObject.GetComponent<Faller>() != null)
                        {
                            shouldFlip = false;
                        }
                    }
                }
            }
        }

        if (clldr.collider.tag == "OppBullets")
        {
            bool isBossProj = clldr.gameObject.GetComponent<BossProjectile>() == null;

            if (isBossProj)
            {
                if (!isInvincible)
                {
                    TakeDamage(clldr.collider.gameObject.GetComponent<Bullet>().damage);
                    shouldFlip = true;
                }

                Destroy(clldr.gameObject);
            }
        }

        if (clldr.gameObject.tag == "OppShield")
        {
            Shield shield = clldr.gameObject.GetComponent<Shield>();

            if (!shield.transform.parent.gameObject.GetComponent<Opp>().isDead)
            {
                Knockback(shield.transform.position, shield.knockbackForce, shield.knockbackForceScale);
                
                if (!isInvincible)
                {
                    TakeDamage(shield.damage);
                }
            }
        }

        if (clldr.gameObject.tag == "Projectiles")
        {
            if (!isInvincible)
            {
                TakeDamage(clldr.gameObject.transform.GetChild(1).gameObject.GetComponent<Bullet>().damage);
            }
        }
    }       

    void OnParticleCollision (GameObject col)
    {
        if (col.gameObject.GetComponent<DustWave>() != null)
        {
            DustWave dustWave = col.gameObject.GetComponent<DustWave>();
            Knockback(dustWave.transform.position, dustWave.blowForce, 1f);
            TakeDamage(dustWave.damage);
        }

        if (col.gameObject.GetComponent<PoisonMist>() != null)
        {
            if (!poisoned)
            {
                PoisonMist poisonMist = col.GetComponent<PoisonMist>();
                print("POISONED!");
                StartCoroutine(Poison(poisonMist.damage));
            }
        }
    }

    void AfterDeath ()
    {
        canMove = false;
        cameraMain.isFollowingPlayer = false;
        canRoll = true;

        if (shouldFlip)
        {
            Flip();
        }

        rotation = transform.rotation;
        deathFinished = true;

        lives--;
        hearts.heartCount = lives;
        hearts.RemoveHearts();
        
        
        if (lives > 0)
        {
            Invoke("Respawn", 2f);
        } else 
        {
            gameCtrlr.gameOver = true;
        }
    }

    public void DisablePowerup (IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }

    void CompleteCooldown ()
    {
        isCooling = true;

        if (particles.isPlaying)
        {
            particles.Stop();
        }
    }

    void Dash ()
    {
        dashEnabled = false;

        for (int i = 0; i < 5; i++)
        {
            rb.AddRelativeForce(Vector3.forward * 10 * speed * Time.deltaTime);
        }

        StartCoroutine(ReenableDash(dashCooldown));
    }

    void ListenForDash ()
    {
        if (Input.GetKeyDown(KeyCode.W) && dashEnabled)
        {
            tapCount++;
            print(tapCount);

            if (tapCount == 1)
            {
                firstTapTime = Time.time;
            }
            else
            {
                if (Time.time - firstTapTime <= tapInterval)
                {
                    
                }

                tapCount = 0;
            }
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            if (tapCount == 1)
            {
                if (Time.time - firstTapTime > tapInterval)
                {
                    tapCount = 0;
                }
            }
        }
    }

    IEnumerator ReenableDash (float delay)
    {
        yield return new WaitForSeconds(delay);
        dashEnabled = true;
    }

    void Flip ()
    {
        for (int i = 0; i < 10; i++)
        {
            rb.AddForce(0, 5000 * Time.deltaTime ,0);
            rb.AddRelativeForce(0, 0, -5000 * Time.deltaTime);
        }
        
        flip.Play();
    }

    void Jump ()
    {
        if (Input.GetKey(KeyCode.Space) && grounded)
        {
            rb.AddForce(0, jumpForce * Time.deltaTime, 0, ForceMode.VelocityChange);
            grounded = false;
            jumped = true;
        }
    }

    void KillIfOffMap ()
    {
        if (transform.position.y <= -3)
        {
            isDead = true;
            shouldFlip = false;
        }
    }

    public void Knockback (Vector3 colliderPos, float knockbackForce, float knockbackForceScale)
    {
        Vector3 direction = transform.position - colliderPos;
        rb.AddForce(
            direction.x * knockbackForce * knockbackForceScale * Time.deltaTime, 
            0,
            direction.z * knockbackForce * knockbackForceScale * Time.deltaTime
        );
    }

    void Move ()
    {
        if (Input.GetKey("w"))
        {
            rb.AddRelativeForce(Vector3.forward * speed * Time.deltaTime);
        }
        if (Input.GetKey("s"))
        {
            rb.AddRelativeForce(Vector3.forward * -speed * Time.deltaTime);
        }
    }

    void MoveLaterally (int direction)
    {
        rb.AddRelativeForce(Vector3.right * direction * speed * Time.deltaTime);
    }

    IEnumerator Poison (float damage)
    {
        poisoned = true;

        TakeDamage(damage);

        for (int i = 0; i < poisonDuration - 1; i++)
        {
            yield return new WaitForSeconds(poisonInterval);
            TakeDamage(damage);
        }

        poisoned = false;
    }

    void ResetDeathSettings () {
        deathFinished = false;
        canMove = true;
        canRoll = false;
    }

    void Respawn ()
    {
        justSpawned = true;
        Vector3 spawnPt = SetSpawnPt();
        transform.position = new Vector3(spawnPt.x, 0, spawnPt.z);
        ResetDeathSettings();
        isDead = false;
        shouldFlip = true;
        health = 1;
        cameraMain.GetComponent<FollowPlayer>().isFollowingPlayer = true;
    }

    void RotateLateralMvt ()
    {
        if (Input.GetKey(KeyCode.D))
        {   
            if (Input.GetKey(KeyCode.LeftShift))
            {
                MoveLaterally(1);
            } else
            {
                rotation *= Quaternion.Euler(Vector3.up * rotSpeed);
            }
        }

        if (Input.GetKey(KeyCode.A))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                MoveLaterally(-1);
            } else
            {
                rotation *= Quaternion.Euler(Vector3.up * -rotSpeed);
            }
        }
    }

    Vector3 SetSpawnPt ()
    {
        Vector3 spawnPt = new Vector3(Random.Range(-80, 80), 0, Random.Range(-80, 80));
        bool goodSpawnPt = false;
        int iteration = 0;

        while (!goodSpawnPt && iteration < 20)
        {
            iteration++;

            if (Physics.OverlapSphere(spawnPt, spawnSafeZone).Length <= 1)
            {
                goodSpawnPt = true;
                break;
            }

            spawnPt = new Vector3(Random.Range(-80, 80), 0, Random.Range(-80, 80));
        } 

        return spawnPt;
    }

    void Shoot ()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            if (bulletEnabled)
            {
                bulletEnabled = false;

                if (tripleMode)
                {
                    GameObject[] bullets = {
                        Instantiate(projectile, firePoint.transform.position, transform.rotation * Quaternion.Euler(0, -60, 0)),
                        Instantiate(projectile, firePoint.transform.position, transform.rotation),
                        Instantiate(projectile, firePoint.transform.position, transform.rotation * Quaternion.Euler(0, 60, 0))
                    };

                    foreach (GameObject bullet in bullets)
                    {
                        bullet.GetComponent<Projectile>().shot = true;
                        bullet.GetComponent<Projectile>().shooter = this.gameObject;
                        bullet.GetComponent<Projectile>().fireRange = fireRange;
                        bullet.GetComponent<Projectile>().isProjectileExplosive = isProjectileExplosive;

                        for (int i = 0; i < 15; i++)
                        {
                            bullet.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 10000 * Time.deltaTime);
                        }
                    }
                } else
                {
                    Projectile bullet = Instantiate(
                        projectile, 
                        firePoint.transform.position, 
                        transform.rotation
                    ).GetComponent<Projectile>();

                    bullet.shot = true;
                    bullet.shooter = this.gameObject;
                    bullet.fireRange = fireRange;
                    bullet.isProjectileExplosive = isProjectileExplosive;
                    MoveBullet(bullet, 10000);

                Invoke("ReenableBullet", fireRate);
            }
            
        }
    }

    void MoveBullet (GameObject obj, float bulletSpd)
    {
        for (int i = 0; i < 15; i++)
            {
                obj.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * bulletSpd * Time.deltaTime);
            }
        }
    }

    void TripleShoot ()
    {
        Projectile[] bullets = new Projectile[3];

        for (int i = 0; i < bullets.Length; i++)
        {
            bullets[i] = Instantiate(
                projectile, 
                firePoint.transform.position + transform.right * ((i-1) * 2), 
                Quaternion.Euler(0, i*60-60, 0)
            ).GetComponent<Projectile>();
            
            bullets[i].gameObject.GetComponent<Rigidbody>().AddRelativeForce(0, 0, 10000 * Time.deltaTime);
        }
    }

    void ReenableBullet ()
    {
        bulletEnabled = true;
    }

    void Straighten ()
    {
        if (!canRoll)
        {
            rotation[0] = 0;
            rotation[2] = 0;
        }
    }

    void TakeDamage (float damage)
    {
        if (damage >= health)
        {
            health = 0;
            isDead = true;
        } else
        {
            health -= damage;
            healthBar.transform.localScale = new Vector3(
                health, 
                healthBar.transform.localScale.y, 
                healthBar.transform.localScale.z
            );
        }
    }

    void ToggleJustSpawned ()
    {
        if (justSpawned)
        {
            StartCoroutine(DisableJustSpawned(justSpawnedDuration));
        }
    }

    IEnumerator DisableJustSpawned (float delay)
    {
        yield return new WaitForSeconds(delay);
        justSpawned = false;
    }
}
