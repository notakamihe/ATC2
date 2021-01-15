using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class NewPlayer : Character, IShooter, IJumper
{
    public Joystick joystick;
    public FollowPlayer cameraMain;
    public GameObject gun;
    public Button dashBtn;

    public Projectile projectile;

    public AudioSource hurtAudio;
    public AudioSource dashAudio;
    AudioSource gameMusic;

    public int lives = 3;

    public float dashRate = 3.0f;
    public float fireRate = 1.0f;
    public float fireRange = 0.5f;
    public float bulletDmgMod = 1.0f;

    [HideInInspector] public bool isInvincible = false;
    [HideInInspector] public bool isProjectileExplosive = false;
    [HideInInspector] public bool tripleMode = false;

    bool isPoisoned = false;
    bool isRespawning = false;

    float lastDash = 5.0f;
    float lastShot = 0.0f;
    Vector3 firePoint;

    Gyroscope gyroscope;

    private void Start()
    {
        base.Start();

        gyroscope = Input.gyro;
        gyroscope.enabled = true;
        gameMusic = Singleton.instance.audioManager.music;
        dashBtn.gameObject.SetActive(!Preferences.IsDashKinetic);
    }

    private void FixedUpdate()
    {
        base.Update();

        if (isDead)
        {
            Die();
        }

        if (Singleton.instance.level.status == Level.LevelStatus.Won)
            gameMusic.Stop();

        firePoint = gun.transform.position + gun.transform.forward * 1.5f;

        DetectForJump();

        if (Physics.OverlapSphere(transform.position, 5.0f).Any(c => c.TryGetComponent(out DustWave dustWave)))
        {
            Jump(jumpForce);
        }

        if (Time.time > 5.0f && Preferences.IsDashKinetic)
        {
            if (gyroscope.attitude.x >= -0.2f && gyroscope.attitude.x <= 0.0f && Input.acceleration.z <= -0.9f)
            {
                Dash();
            }
        }

        Move();
    }

    public void Dash ()
    {
        if (Time.time > lastDash + dashRate)
        {
            lastDash = Time.time;
            dashAudio.Play();

            for (int i = 0; i < 5; i++)
            {
                rb.AddRelativeForce(Vector3.forward * 10 * speed * Time.deltaTime);
            }
        }
    }

    public void DetectForJump ()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 8.0f, LayerMask.GetMask("Obstacle")))
        {
            if (Vector3.Angle(transform.forward, rb.velocity) <= 35 && rb.velocity.magnitude > 5.0f)
            {
                Jump(jumpForce);
            }
        }
    }

    protected override void Die()
    {
        if (!isRespawning)
        {
            lives--;
            gameMusic.Pause();

            if (shouldFlip)
                Flip();

            if (lives > 0)
            {
                StartCoroutine(Respawn());
            }
            else
            {
                rb.isKinematic = true;
                this.enabled = false;
                Singleton.instance.level.status = Level.LevelStatus.Over;
            }
        }
    }

    public void DisablePowerup(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }

    public void FireShot (Vector3 direction)
    {
        Projectile shot = Instantiate(projectile, firePoint, transform.rotation);

        gun.GetComponent<AudioSource>().Play();

        shot.isShot = true;
        shot.shooter = this.gameObject;
        shot.fireRange = fireRange;
        shot.isProjectileExplosive = isProjectileExplosive;

        for (int i = 0; i < 15; i++)
            shot.GetComponent<Rigidbody>().AddForce(
                (direction * 3000.0f) * Time.deltaTime);
    }

    public void FireShot(Projectile shot, Vector3 direction)
    {
        gun.GetComponent<AudioSource>().Play();

        shot.isShot = true;
        shot.shooter = this.gameObject;
        shot.fireRange = fireRange;
        shot.isProjectileExplosive = isProjectileExplosive;

        for (int i = 0; i < 15; i++)
            shot.GetComponent<Rigidbody>().AddRelativeForce(
                (direction * 3000.0f) * Time.deltaTime);
    }

    void GetKnockedBack (Vector3 otherPos, float force)
    {
        Vector3 direction = transform.position - otherPos;
        rb.AddForce(direction * force * Time.deltaTime);
    }

    IEnumerator GetPosioned(float dmg, float duration)
    {
        isPoisoned = true;

        TakeDamage(dmg);

        for (int i = 0; i < duration - 1; i++)
        {
            yield return new WaitForSeconds(0.8f);
            TakeDamage(dmg);
        }

        isPoisoned = false;
    }

    public void Jump(float force)
    {
        if (grounded)
        {
            rb.AddForce(Vector3.up * force);
        }
    }

    protected void Move()
    {
        rb.AddRelativeForce(Vector3.forward * joystick.Vertical * speed * Time.deltaTime);
        RotateLateralMvt();
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        if (collision.collider.TryGetComponent(out NewOpp newOpp))
        {
            if (!newOpp.isDead)
            {
                GetKnockedBack(newOpp.transform.position, newOpp.knockbackForce);

                if (!isInvincible && !isDead)
                {
                    TakeDamage(newOpp.meleeDamage);
                }
            }
        }

        if (collision.collider.TryGetComponent(out Bullet bullet))
        {
            if (!isInvincible)
                TakeDamage(bullet.damage);
        }

        if (collision.collider.TryGetComponent(out NewShield shield))
        {
            if (!shield.GetComponentInParent<NewOpp>().isDead)
            {
                GetKnockedBack(shield.transform.position, shield.knockbackForce);

                if (!isInvincible)
                    TakeDamage(shield.damage);
            }
        }

        if (collision.collider.TryGetComponent(out Cannonball cannonball))
        {
            if (!isInvincible)
                TakeDamage(cannonball.damage);
        }
    }

    protected override void OnCollisionExit(Collision collision)
    {
        base.OnCollisionExit(collision);
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.TryGetComponent(out DustWave dustWave))
        {
            GetKnockedBack(dustWave.transform.position, dustWave.blowForce);
            TakeDamage(dustWave.damage);
        }

        if (other.TryGetComponent(out PoisonMist poison) && !isPoisoned)
        {
            StartCoroutine(GetPosioned(poison.damage, poison.duration));
        }
    }

    IEnumerator Respawn()
    {
        isRespawning = true;

        yield return new WaitForSeconds(3.0f);

        health = maxHealth;
        isDead = false;
        gameMusic.UnPause();

        transform.SetParent(null);
        animParent.SetParent(transform);

        Spawn();

        isRespawning = false;
    }

    void RotateLateralMvt()
    {
        rb.AddRelativeForce(Vector3.right * Input.acceleration.x * speed * Time.deltaTime);

        if (joystick.Vertical > -0.35f)
            transform.rotation *= Quaternion.Euler(Vector3.up * joystick.Horizontal * rotSpeed * Time.deltaTime);
    }

    public void Shoot()
    {
        if (Time.time > lastShot + fireRate)
        {
            lastShot = Time.time;

            if (tripleMode)
            {
                TripleShoot();
            }
            else
            {
                if (Physics.BoxCast(transform.position, new Vector3(4, 12, 4) * 0.5f, transform.forward,
                    out RaycastHit hit, transform.rotation, 30.0f))
                {
                    if (hit.collider.GetComponent(typeof(NewOpp)) || hit.collider.GetComponentInParent(typeof(NewOpp)))
                    {
                        NewOpp opp = (NewOpp)hit.collider.GetComponentInParent(typeof(NewOpp));
                        FireShot((opp.transform.position - transform.position).normalized);
                    }
                    else
                    {
                        FireShot(transform.forward);
                    }
                }
                else
                    FireShot(transform.forward);
            }
        }
    }

    void Spawn(int iteration = 0)
    {
        Vector3 spawnPoint = new Vector3(Random.Range(-80, 80), 0, Random.Range(-80, 80));

        Collider[] spawnSphere = Physics.OverlapSphere(transform.position, 5.0f);

        if (iteration <= 20 && spawnSphere.Any(c => c.CompareTag("Obstacle")|| c.TryGetComponent(out NewOpp opp)))
        {
            Spawn(iteration + 1);
        } else
        {
            transform.position = spawnPoint;

            animParent.localRotation = Quaternion.Euler(0, 0, 0);
            transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        }
    }

    public override void TakeDamage(float dmg)
    {
        hurtAudio.Play();

        base.TakeDamage(dmg);
    }

    void TripleShoot ()
    {
        float angleBetweem = 9.0f;

        for (int i = 0; i < 3; i++)
        {
            Projectile shot = Instantiate(projectile, firePoint + transform.right * (i-1) * 0.5f, Quaternion.identity);
            FireShot(shot, Quaternion.AngleAxis(angleBetweem * i - angleBetweem, Vector3.up) * transform.forward);
        }
    }
}