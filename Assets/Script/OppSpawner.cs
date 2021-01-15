using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class OppSpawner : MonoBehaviour
{
    public NewOpp[] oppPrefabs;
    NewPlayer player;

    public int numOppsMax = 10;
    public int numOppsSimultaneousMax = 3;
    int numOpps;
    int numOppsSimultaneous;
    [HideInInspector] public int numDeadOpps { get; private set; }

    List<NewOpp> aliveOpps = new List<NewOpp>();

    float spawnRate;
    float lastSpawn;

    bool AreAllOppsDead
    {
        get
        {
            return numDeadOpps >= numOppsMax;
        }
    }

    private void Start()
    {
        player = Singleton.instance.player;
    }

    private void Update()
    {
        if (AreAllOppsDead)
        {
            Singleton.instance.level.status = Level.LevelStatus.Won;
            this.enabled = false;
        }

        try
        {
            foreach (NewOpp opp in aliveOpps)
            {
                if (opp == null || opp.isDead)
                {
                    numDeadOpps++;
                    numOppsSimultaneous--;
                    aliveOpps.Remove(opp);
                }
            }
        } catch (InvalidOperationException)
        {
        }

        if (numOpps < numOppsMax && numOppsSimultaneous < numOppsSimultaneousMax)
        {
            if (Time.time > lastSpawn + spawnRate)
            {
                lastSpawn = Time.time;
                spawnRate = UnityEngine.Random.Range(2.0f, 12.0f);
                NewOpp oppPrefab = oppPrefabs[UnityEngine.Random.Range(0, oppPrefabs.Length)];
                Spawn(oppPrefab);
            }
        }
    }

    void Spawn (NewOpp oppPrefab, int iteration = 0)
    {
        Vector3 randPos = new Vector3(UnityEngine.Random.Range(-90.0f, 90.0f), 0, 
            UnityEngine.Random.Range(-90.0f, 90.0f));

        Collider[] surroundingsForPlayer = Physics.OverlapSphere(randPos, 10.0f);
        Collider[] surroundingsForObstacles = Physics.OverlapSphere(randPos, 2.0f);

        if ((surroundingsForPlayer.Any(s => s.gameObject == player.gameObject) || 
            surroundingsForObstacles.Any(s => s.gameObject.CompareTag("Obstacle"))) && iteration <= 20)
        {
            Spawn(oppPrefab, iteration + 1);
        }
        else
        {
            NewOpp opp = Instantiate(oppPrefab, randPos, Quaternion.identity);
            numOpps++;
            numOppsSimultaneous++;
            aliveOpps.Add(opp);
        }
    }
}