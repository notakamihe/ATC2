using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : MonoBehaviour
{
    public PowerUp powerUp;
    public float playerFireRange;
    public float playerBulletDmgMod;
    public float fireRangeScale = 3f;
    public float bulletDmgModScale = 2f;

    void Update()
    {
        if (powerUp.initialized == false)
        {
            powerUp.initialized = true;
            playerFireRange = powerUp.player.fireRange;
            playerBulletDmgMod = powerUp.player.bulletDmgMod;
        }
    }

    void OnTriggerEnter (Collider col)
    {
        if (col.gameObject == powerUp.player.gameObject || 
            col.gameObject.tag == "Bullets" ||
            col.gameObject.transform.parent.gameObject == powerUp.player.gameObject)
        {
            powerUp.player.fireRange *= fireRangeScale;
            powerUp.player.bulletDmgMod *= bulletDmgModScale;
            powerUp.player.snipeMode = true;
            powerUp.player.DisablePowerup(ResetFireRange(powerUp.duration));
        }
        
        Destroy(this.gameObject);
    }

    IEnumerator ResetFireRange (float delay)
    {
        yield return new WaitForSeconds(delay);
        powerUp.player.fireRange = playerFireRange;
        powerUp.player.bulletDmgMod = playerBulletDmgMod;
        powerUp.player.snipeMode = false;
    }
}
