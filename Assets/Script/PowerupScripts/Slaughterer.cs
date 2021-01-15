using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slaughterer : PowerUp
{
    public float playerBulletDmgMod;
    public float bulletDmgModScale = 3f;

    new void Start()
    {
        base.Start();

        playerBulletDmgMod = player.bulletDmgMod;
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
            player.bulletDmgMod *= bulletDmgModScale;
            player.DisablePowerup(ResetDmg(duration));
        }

        Destroy(this.gameObject);
    }

    IEnumerator ResetDmg(float delay)
    {
        yield return new WaitForSeconds(delay);
        player.bulletDmgMod = playerBulletDmgMod;
    }
}
