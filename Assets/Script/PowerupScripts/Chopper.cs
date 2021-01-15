using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chopper : PowerUp
{
    public float playerFireRate;
    public float fireRateScale = .2f;

    new void Start()
    {
        base.Start();

        playerFireRate = player.fireRate;
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
            player.fireRate = Mathf.Clamp(player.fireRate * fireRateScale, 0.2f, 1.0f);
            player.DisablePowerup(ResetFireRate(duration));
        }

        Destroy(this.gameObject);
    }

    IEnumerator ResetFireRate(float delay)
    {
        yield return new WaitForSeconds(delay);
        player.fireRate = playerFireRate;
    }
}
