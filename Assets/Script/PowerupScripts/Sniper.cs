using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : PowerUp
{
    public float playerFireRange;
    public float playerBulletDmgMod;
    public float fireRangeScale = 3f;
    public float bulletDmgModScale = 2f;

    new void Start()
    {
        base.Start();

        playerFireRange = player.fireRange;
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
            player.fireRange *= fireRangeScale;
            player.bulletDmgMod *= bulletDmgModScale;
            player.cameraMain.offset *= 2.0f;
            player.DisablePowerup(ResetFireRange(duration, 2.0f));
        }

        Destroy(this.gameObject);
    }

    IEnumerator ResetFireRange(float delay, float offsetScale)
    {
        yield return new WaitForSeconds(delay);
        player.fireRange = playerFireRange;
        player.bulletDmgMod = playerBulletDmgMod;
        player.cameraMain.offset *= (1.0f / offsetScale);
    }
}
