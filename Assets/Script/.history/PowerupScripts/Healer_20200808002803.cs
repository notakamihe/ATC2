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
        float playerHealth = player.health;

        if (playerHealth + healAmount > 1)
        {
            player.health = 1;
        } else
        {
            player.health += healAmount;
        }

        Destroy(this.gameObject);
    }
}
