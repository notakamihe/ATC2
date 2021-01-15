using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class PowerupSpawner : MonoBehaviour
{
    public PowerUp[] powerupPrefabs;
    NewPlayer player;

    public int numPowerupsMax = 10;
    public int numPowerupsSimultaneousMax = 5;
    int numPowerups;
    int numPowerupsSimultaneous;

    List<PowerUp> powerups = new List<PowerUp>();

    float spawnRate;
    float lastSpawn;

    private void Start()
    {
        player = Singleton.instance.player;
    }

    private void Update()
    {
        try
        {
            foreach (PowerUp powerup in powerups)
            {
                if (powerup == null)
                {
                    numPowerupsSimultaneous--;
                    powerups.Remove(powerup);
                }
            }
        }
        catch (InvalidOperationException)
        {
        }

        if (numPowerups < numPowerupsMax && numPowerupsSimultaneous < numPowerupsSimultaneousMax)
        {
            if (Time.time > lastSpawn + spawnRate)
            {
                lastSpawn = Time.time;
                spawnRate = UnityEngine.Random.Range(2.0f, 12.0f);
                PowerUp powerupPrefab = powerupPrefabs[UnityEngine.Random.Range(0, powerupPrefabs.Length)];
                Spawn(powerupPrefab);
            }
        }
    }

    void Spawn(PowerUp powerupPrefab)
    {
        Vector3 randPos = new Vector3(UnityEngine.Random.Range(-90.0f, 90.0f), 0,
            UnityEngine.Random.Range(-90.0f, 90.0f));

        PowerUp powerup = Instantiate(powerupPrefab, randPos, Quaternion.identity);
        numPowerups++;
        numPowerupsSimultaneous++;
        powerups.Add(powerup);
    }
}
