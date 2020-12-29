using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slaughterer : MonoBehaviour
{
    public PowerUp powerUp;
    public float playerBulletDmgMod;
    public float bulletDmgModScale = 3f;

    void Update()
    {
        if (powerUp.initialized == false)
        {
            powerUp.initialized = true;
            playerBulletDmgMod = powerUp.player.bulletDmgMod;
        }
    }

    void OnTriggerEnter ()
    {
        powerUp.player.bulletDmgMod *= bulletDmgModScale;
        powerUp.player.DisablePowerup(IncrDmg(powerUp.duration));
        Destroy(this.gameObject);
    }

    IEnumerator IncrDmg (float delay)
    {
        yield return new WaitForSeconds(delay);
        powerUp.player.bulletDmgMod = playerBulletDmgMod;
    }
}
