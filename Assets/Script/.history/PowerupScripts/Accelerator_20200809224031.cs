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

    void OnTriggerEnter (Collider col)
    {
        if (col.gameObject == powerUp.player || col.gameObject.tag == "Bullets")
        {
            powerUp.player.speed *= speedScale;
            powerUp.player.DisablePowerup(ResetSpd(powerUp.duration));
        }
        
        Destroy(this.gameObject);
    }

    IEnumerator ResetSpd (float delay)
    {
        yield return new WaitForSeconds(delay);
        powerUp.player.speed = playerSpeed;
    }
}
