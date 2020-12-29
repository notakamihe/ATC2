using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slaughterer : MonoBehaviour
{
    public PowerUp powerUp;
    public float playerbulletDmgMod;
    public float bulletDmgModScale = 3f;

    void Update()
    {
        if (powerUp.initialized == false)
        {
            powerUp.initialized = true;
            playerFireRange = powerUp.player.fireRange;
            playerRotSpeed = powerUp.player.rotSpeed;
            playerbulletDmgMod = powerUp.player.bulletDmgMod;
        }
    }

    void OnTriggerEnter ()
    {
        powerUp.player.fireRange *= fireRangeScale;
        powerUp.player.rotSpeed *= rotSpeedScale;
        powerUp.player.bulletDmgMod *= bulletDmgModScale;
        powerUp.player.DisablePowerup(IncrFireRange(powerUp.duration));
        Destroy(this.gameObject);
    }

    IEnumerator IncrFireRange (float delay)
    {
        yield return new WaitForSeconds(delay);
        powerUp.player.fireRange = playerFireRange;
        powerUp.player.rotSpeed = playerRotSpeed;
        powerUp.player.bulletDmgMod = playerbulletDmgMod;
    }
}
