using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tripler : PowerUp
{
    new void Start()
    {
        base.Start();
    }

    new void Update()
    {
        base.Update();
    }

    void OnTriggerEnter (Collider col)
    {
        base.OnTriggerEnter();

        if (col.gameObject == player.gameObject || col.gameObject.CompareTag("Bullets") ||
            col.transform.parent == player.transform)
        {
            player.tripleMode = true;
            player.DisablePowerup(ResetExplosive(duration));
        }
        
        Destroy(this.gameObject);
    }

    IEnumerator ResetExplosive (float delay)
    {
        yield return new WaitForSeconds(delay);
        player.tripleMode = false;
    }
}
