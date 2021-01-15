using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : PowerUp
{
    public float healAmt = 50.0f;

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
            Heal();
        }

        Destroy(this.gameObject);
    }

    void Heal()
    {
        player.health = Mathf.Min(player.health + healAmt, player.maxHealth);
    }
}
