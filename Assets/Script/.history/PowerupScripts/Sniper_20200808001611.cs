using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : MonoBehaviour
{
    public PowerUp powerUp;
    public float playerFireRange;
    public float playerRotSpeed;
    public float playerBulletDmgMod;
    public float fireRangeScale = 3f;
    public float rotSpeedScale = .2f;
    public float bulletDmgModScale = 2f;

    void Update()
    {
        if (powerUp.initialized == false)
        {
            powerUp.initialized = true;
            playerFireRange = powerUp.player.fireRange;
            playerRotSpeed = powerUp.player.rotSpeed;
            playerBulletDmgMod = powerUp.player.bulletDmgMod;
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
        powerUp.player.bulletDmgMod = playerBulletDmgMod;
    }
}
