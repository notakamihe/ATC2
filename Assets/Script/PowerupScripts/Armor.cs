using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : PowerUp
{
    new void Start()
    {
        base.Start();
    }

    new void Update()
    {
        base.Update();
    }

    void OnTriggerEnter(Collider col)
    {
        base.OnTriggerEnter();

        if (col.gameObject == player.gameObject || col.gameObject.CompareTag("Bullets") ||
            col.transform.parent == player.transform)
        {
            player.isInvincible = true;
            player.DisablePowerup(ResetExplosive(duration));
        }

        Destroy(this.gameObject);
    }

    IEnumerator ResetExplosive(float delay)
    {
        yield return new WaitForSeconds(delay);
        player.isInvincible = false;
    }
}
