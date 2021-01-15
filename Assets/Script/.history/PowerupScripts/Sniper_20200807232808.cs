using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : MonoBehaviour
{
    public PowerUp powerUp;
    public float playerFireRange;
    public float playerRotSpeed;
    public float fireRangeScale = 3f;
    public float rotSpeedScale = .2f;

    void Update()
    {
        if (powerUp.initialized == false)
        {
            powerUp.initialized = true;
            playerFireRange = powerUp.player.fireRange;
            playerRotSpeed = powerUp.player.fireRate;
        }
    }

    void OnTriggerEnter ()
    {
        powerUp.player.fireRate *= .2f;
        powerUp.player.DisablePowerup(IncrFireRate(powerUp.duration));
        Destroy(this.gameObject);
    }

    IEnumerator IncrFireRate (float delay)
    {
        yield return new WaitForSeconds(delay);
        powerUp.player.fireRate = playerFireRange;
    }
}
