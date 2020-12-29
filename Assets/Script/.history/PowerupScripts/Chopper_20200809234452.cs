using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chopper : MonoBehaviour
{
    public PowerUp powerUp;
    public float playerFireRate;
    public float fireRateScale = .2f;

    void Update()
    {
        if (powerUp.initialized == false)
        {
            powerUp.initialized = true;
            playerFireRate = powerUp.player.fireRate;
        }
    }

    void OnTriggerEnter (Collider col)
    {
        print(col.gameObject);

        if (col.gameObject == powerUp.player.gameObject || 
            col.gameObject.tag == "Bullets" ||
            col.gameObject.transform.parent.gameObject == powerUp.player.gameObject)
        {
            powerUp.player.fireRate *= .2f;
            powerUp.player.DisablePowerup(ResetFireRate(powerUp.duration));
        }

        Destroy(this.gameObject);
    }

    IEnumerator ResetFireRate (float delay)
    {
        yield return new WaitForSeconds(delay);
        powerUp.player.fireRate = playerFireRate;
    }
}
