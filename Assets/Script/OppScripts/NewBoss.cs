using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class NewBoss : NewOpp, IShooter
{
    [Header("Boss fields")]

    public Cannonball ammo;
    public PoisonMist poisonMistPrefab;
    public DustWave dustWavePrefab;

    public GameObject gun;
    public Transform firePoint;
    public AudioSource fallAudio;

    public float radiusClose;
    public float radiusMed;
    public float radiusMax;

    List<Action> closeRangeActions = new List<Action>();
    List<Action> midRangeActions = new List<Action>();
    List<Action> longRangeActions = new List<Action>();

    public NewOpp[] oppArray;
    List<NewOpp> spawnedOpps = new List<NewOpp>();

    float lastMove;
    float moveRate;

    private void Start()
    {
        base.Start();

        radiusMax = playerDetectionRadius;
        radiusMed = playerDetectionRadius * 0.6f;
        radiusClose = playerDetectionRadius * 0.35f;

        closeRangeActions = new List<Action>() { SprayPoison, Pounce, Teleport };
        midRangeActions = new List<Action>() { Shoot, Shockwave, Teleport, Birth };
        longRangeActions = new List<Action>() { Shoot };
    }

    private void FixedUpdate()
    {
        base.FixedUpdate();

        foreach (NewOpp opp in spawnedOpps)
        {
            if (opp == null || opp.isDead)
                spawnedOpps.Remove(opp);
        }
    }

    protected override void Approach()
    {
        if (!IsNearEdges)
            Move();

        if (Time.time > lastMove + moveRate)
        {
            lastMove = Time.time;
            moveRate = UnityEngine.Random.Range(5.0f, 10.0f);

            if (PlayerInRange(radiusClose))
            {
                closeRangeActions[UnityEngine.Random.Range(0, closeRangeActions.Count)]();
            } else if (PlayerInRange(radiusMed))
            {
                midRangeActions[UnityEngine.Random.Range(0, midRangeActions.Count)]();
            }
            else
            {
                longRangeActions[UnityEngine.Random.Range(0, longRangeActions.Count)]();
            }
        }
    }

    void Birth ()
    {
        print("Birth");

        if (UnityEngine.Random.Range(1, 3) == 1)
        {
            if (spawnedOpps.Count < 3)
            {
                NewOpp newOpp = Instantiate(oppArray[UnityEngine.Random.Range(0, oppArray.Length)], 
                    transform.position - Vector3.forward * (transform.localScale.z/1.5f), Quaternion.identity);
                spawnedOpps.Add(newOpp);
            }
        }
    }

    public void FireShot (Vector3 direction)
    {
        Cannonball projectile = Instantiate(ammo, firePoint.position, transform.rotation);

        gun.GetComponent<AudioSource>().Play();

        for (int i = 0; i < 15; i++)
            projectile.rb.AddForce((direction * 90000.0f + Vector3.down * 50) * Time.deltaTime);
    }

    void Pounce ()
    {
        if (grounded && !IsNearEdges)
        {
            rb.AddForce(Vector3.up * jumpForce + DirToPlayer * DistanceFromPlayer * 120.0f * Time.deltaTime, 
                ForceMode.Acceleration);
        }
    }

    void Shockwave()
    {
        print("Dust wave");
        StartCoroutine(CreateShockwave());
    }

    IEnumerator CreateShockwave ()
    {
        if (grounded)
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Acceleration);

        yield return new WaitForSeconds(0.1f);

        while (true)
        {
            if (grounded)
            {
                DustWave dustWave = Instantiate(dustWavePrefab,
                    transform.position - Vector3.up * (transform.localScale.y/2), Quaternion.Euler(-90.0f, 0.0f, 0.0f));
                fallAudio.Play();
                break;
            }

            yield return null;
        }
    }

    public void Shoot ()
    {
        print("Shoot");
        FireShot(DirToPlayer.normalized);
    }

    void SprayPoison()
    {
        print("Poison");
        StartCoroutine(SprayMist());
    }

    IEnumerator SprayMist ()
    {
        PoisonMist poisonMist = Instantiate(poisonMistPrefab,
            transform.position + transform.up * (transform.localScale.y/2), Quaternion.Euler(-90.0f, 0.0f, 0.0f));

        while (poisonMist.partSys.isPlaying)
        {
            print("Spraying");
            
            yield return null;
        }
    }

    void Teleport ()
    {
        print("Teleport");
        Vector3 newPos = player.transform.position - player.transform.forward * 8.0f;
        newPos.y = transform.position.y;
        transform.position = newPos;
    }
}