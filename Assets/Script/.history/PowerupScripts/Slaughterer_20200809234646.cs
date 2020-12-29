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

    void OnTriggerEnter (Collider col)
    {
        if (col.gameObject == powerUp.player.gameObject || 
            col.gameObject.tag == "Bullets" ||
            col.gameObject.transform.parent.gameObject == powerUp.player.gameObject)
        {
            powerUp.player.bulletDmgMod *= bulletDmgModScale;
            powerUp.player.DisablePowerup(ResetDmg(powerUp.duration));
        }
        
        Destroy(this.gameObject);
    }

    IEnumerator ResetDmg (float delay)
    {
        yield return new WaitForSeconds(delay);
        powerUp.player.bulletDmgMod = playerBulletDmgMod;
    }
}
