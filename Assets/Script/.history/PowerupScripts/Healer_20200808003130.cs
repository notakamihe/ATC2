using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : MonoBehaviour
{
    public PowerUp powerUp;
    public float healAmt = .5f;

    void OnTriggerEnter ()
    {
        Heal();
        Destroy(this.gameObject);
    }

    void Heal ()
    {
        float playerHealth = powerUp.player.health;

        if (playerHealth + healAmt > 1)
        {
            powerUp.player.health = 1;
        } else
        {
            powerUp.player.health += healAmt;
        }
    }
}
