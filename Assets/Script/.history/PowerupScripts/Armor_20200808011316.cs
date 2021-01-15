using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : MonoBehaviour
{
    public PowerUp powerUp;

    void OnTriggerEnter ()
    {
        powerUp.player.isInvincible = true;
        powerUp.player.DisablePowerup(ResetExplosive(powerUp.duration));
        Destroy(this.gameObject);
    }

    IEnumerator ResetExplosive (float delay)
    {
        yield return new WaitForSeconds(delay);
        powerUp.player.isInvincible = false;
    }
}
