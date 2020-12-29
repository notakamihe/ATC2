using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tripler : MonoBehaviour
{
    public PowerUp powerUp;

    void OnTriggerEnter (Collider col)
    {
        if (col.gameObject == powerUp.player.gameObject || 
            col.gameObject.tag == "Bullets" ||
            col.gameObject.transform.parent.gameObject == powerUp.player.gameObject)
        {
            powerUp.player.tripleMode = true;
            powerUp.player.DisablePowerup(ResetExplosive(powerUp.duration));
        }
        
        Destroy(this.gameObject);
    }

    IEnumerator ResetExplosive (float delay)
    {
        yield return new WaitForSeconds(delay);
        powerUp.player.tripleMode = false;
    }
}
