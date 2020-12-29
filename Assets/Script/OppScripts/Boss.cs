using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public Opp opp;
    public Follower follower;
    public Player player;
    public GameObject ground;
    public GameObject projectile;
    public GameObject firePt;
    public GameObject[] oppArr;
    public GameObject dustWave;
    public GameObject poisonMist;
    public GameObject gun;
    public AudioSource fall;
    public string[] closeRngMoveset = {"Pounce"};
    public string[] medRngMoveset = {"Birth", "Shoot"};
    public string[] farRngMoveset = {"Shoot"};
    public float coolDownTimeMin = 5;
    public float coolDownTimeMax = 10;
    public float detectRngClose = 7f;
    public float detectRngMed = 25f;
    public float detectRngFar = 40f;
    public float jumpForce = 5000f;
    public float rbDrag = 5f;
    public float bulletSpd = 10000f;
    public float teleportDuration = 10f;
    public float newSpeedTeleport;
    public float speedTeleportMod = 3f;
    public int performChance = 3;
    public int spawnCount = 3;
    public int maxOpps = 5;
    public int oppCount = 0;
    public bool grounded = true;
    public bool createDustWave = false;
    public bool canTeleport = false;
    public bool cooledDown = true;
    public bool jumped = false;

    // Start is called before the first frame update
    void Start()
    {
        ground = GameObject.FindWithTag("Ground");
        newSpeedTeleport = opp.speed * speedTeleportMod;
        fall = GetComponents<AudioSource>()[0];
    }

    // Update is called once per frame
    void Update()
    {
        oppArr = opp.gameCtrlr.oppsArray;
        
        if (!opp.player.justSpawned && !opp.isDead)
        {
            DetectPlayerForMoves();
        }
    }

    void OnCollisionEnter (Collision col)
    {
        if (col.gameObject.tag == "Ground")
        {
            grounded = true;
            opp.rb.drag = rbDrag;

            if (createDustWave)
            {
                follower.enabled = true;
                createDustWave = false;
                Instantiate(
                    dustWave, 
                    new Vector3(transform.position.x, ground.transform.position.y, transform.position.z),
                    Quaternion.Euler(90, 0, 0)
                );
            }

            if (jumped)
            {
                jumped = false;
                fall.Play();
            }
        }

        if (col.collider.tag == "Bullets")
        {
            if (canTeleport)
            {
                print("Teleport!");
                Teleport();
            }
        }
    }

    void DetectPlayerForMoves ()
    {
        if (opp.DistFrmPlayer() <= detectRngClose && !opp.NearBounds())
        {
            PerformMove(closeRngMoveset);
        } else if (opp.DistFrmPlayer() <= detectRngMed && !opp.NearBounds())
        {
            PerformMove(medRngMoveset);
        } else if (opp.DistFrmPlayer() <= detectRngFar && !opp.NearBounds())
        {
            PerformMove(farRngMoveset);
        }
    }

    void PerformMove (string[] moveset)
    {
        if (cooledDown)
        {
            if (Random.Range(1, performChance + 1) == 1)
            {
                cooledDown = false;

                try 
                {
                    Invoke(moveset[Random.Range(0, moveset.Length)], 0f);
                } catch
                { 
                    return;
                }

                StartCoroutine(CoolDown(Random.Range(coolDownTimeMin, coolDownTimeMax)));
            }
        }
    }

    IEnumerator CoolDown (float delay)
    {
        yield return new WaitForSeconds(delay);
        cooledDown = true;
    }

    void Pounce ()
    {
        grounded = false;
        jumped = true;

        for (int i = 0; i < 10; i++)
        {
            opp.rb.AddForce(0, jumpForce, 0);
        }

        StartCoroutine(RemoveDrag(0, jumpForce/10000));
    }

    void Shoot ()
    {
        Debug.Log("PEW!");
        FireBullet();
    }

    void FireBullet ()
    {
        GameObject bullet = Instantiate(
            projectile, 
            firePt.transform.position, 
            Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(opp.player.transform.position - firePt.transform.position),
                30 * Time.deltaTime
            )
        );
        gun.GetComponent<AudioSource>().Play();

        for (int i = 0; i < 10; i++)
        {
            bullet.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * bulletSpd * Time.deltaTime);
        }
    }

    void Birth ()
    {   
        Debug.Log("BIRTHED");
        if (Random.Range(1, 2) == 1)
        {
            if (oppCount < maxOpps)
            {
                Vector3 behindPos = transform.position + transform.forward * -3;

                for (int i = 0; i < spawnCount; i++)
                {
                    Opp opp = Instantiate(
                        oppArr[Random.Range(0, oppArr.Length)], 
                        new Vector3(behindPos.x - Random.Range(-5.0f, 5.0f), 0, behindPos.z),
                        Quaternion.identity
                    ).GetComponent<Opp>();
                    opp.spawner = this.gameObject;
                    oppCount++;
                }
            }
        }
    }

    void JumpForDustWave()
    {
        createDustWave = true;
        follower.enabled = false;
        Pounce();
    }

    void EnableTeleport ()
    {
        canTeleport = true;
        StartCoroutine(DisableTeleport(teleportDuration, newSpeedTeleport));
    }

    void Teleport ()
    {
        transform.rotation = opp.FixRotToPlayer();
        transform.position = opp.player.transform.position + opp.player.transform.forward * -(transform.localScale.z + 6);
        opp.speed = newSpeedTeleport;
    }

    void SprayPoison ()
    {
        Debug.Log("SPRAY");
        follower.enabled = false;
        PoisonMist poison = Instantiate(
            poisonMist, 
            transform.position + transform.up * (transform.localScale.y),
            Quaternion.Euler(-90, 0, 0)
        ).GetComponent<PoisonMist>();
        StartCoroutine(EnableFollower(poison.partSys.main.duration));
    }

    IEnumerator RemoveDrag (float drag, float delay)
    {
        yield return new WaitForSeconds(delay);
        opp.rb.drag = drag;
    }

    IEnumerator DisableTeleport (float delay, float speedDouble)
    {
        yield return new WaitForSeconds(delay);
        canTeleport = false;

        if (opp.speed >= speedDouble)
        {
            opp.speed /= speedTeleportMod;
        }
    }

    IEnumerator EnableFollower (float delay)
    {
        yield return new WaitForSeconds(delay);
        follower.enabled = true;
    }

    void OnDrawGizmosSelected ()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, detectRngClose);
        Gizmos.DrawWireSphere(transform.position, detectRngMed);
        Gizmos.DrawWireSphere(transform.position, detectRngFar);
        Gizmos.DrawWireSphere(transform.position, opp.detectionRadius);
    }
}
