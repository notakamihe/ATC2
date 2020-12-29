using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charger : MonoBehaviour
{
    public AudioSource charge;
    public Opp opp;
    public float detectRadiusCharge;
    public float coolDownTime = 2f;
    public bool cooledDown = true;

    // Start is called before the first frame update
    void Start()
    {
        detectRadiusCharge = opp.detectionRadius / 2;
        charge = GetComponents<AudioSource>()[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (!opp.isDead)
        {
            if (!opp.player.isDead)
            {
                FollowPlayer();
            }
        }
    }

    void FollowPlayer ()
    {
        if (opp.DistFrmPlayer() <= opp.detectionRadius)
        {
            opp.FixRotToPlayer();

            if (Vector3.Distance(opp.player.transform.position, transform.position) <= detectRadiusCharge && cooledDown)
            {
                Charge();
            } else
            {
                opp.rb.AddRelativeForce(0, 0, opp.speed * Time.deltaTime);
            }
        }
    }

    void Charge ()
    {
        charge.Play();
        opp.rb.MovePosition(opp.player.transform.position + opp.player.transform.forward * 3);
        cooledDown = false;
        StartCoroutine(StopChargeSnd(2f));
        StartCoroutine(CoolDown(coolDownTime));
    }

    IEnumerator StopChargeSnd (float delay)
    {
        yield return new WaitForSeconds(delay);
        charge.Stop();
    }
 
    IEnumerator CoolDown (float delay)
    {
        yield return new WaitForSeconds(delay);
        cooledDown = true;
    }

    void OnDrawGizmosSelected ()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, opp.detectionRadius);
        Gizmos.DrawWireSphere(transform.position, detectRadiusCharge);
    }
}
