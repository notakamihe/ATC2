using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accelerator : MonoBehaviour
{
    public PowerUp powerUp;
    public float playerSpeed;
    public float speedScale = 2f;

    void Update()
    {
        if (powerUp.initialized == false)
        {
            powerUp.initialized = true;
            playerSpeed = powerUp.player.speed;
        }
    }

    void OnTriggerEnter ()
    {
        powerUp.player.speed *= speedScale;
        powerUp.player.DisablePowerup(IncrSpd(powerUp.duration));
        Destroy(this.gameObject);
    }

    IEnumerator IncrSpd (float delay)
    {
        yield return new WaitForSeconds(delay);
        powerUp.player.bulletDmgMod = playerBulletDmgMod;
    }
}
