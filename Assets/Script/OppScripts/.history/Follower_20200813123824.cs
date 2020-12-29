using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public Opp opp;
    public bool playerFound = false;
    public bool movementBounds = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!opp.isDead && !opp.player.isDead)
        {
            CheckBounds();
            FollowPlayer();
        }
    }

    void FollowPlayer ()
    {
        if (opp.DistFrmPlayer() <= opp.detectionRadius)
        {
            playerFound = true;

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(opp.player.transform.position - transform.position),
                opp.rotSpeed * Time.deltaTime
            );
            opp.rb.AddRelativeForce(0, 0, opp.speed * Time.deltaTime);
        } else
        {
            playerFound = false;
        }
    }

    void CheckBounds ()
    {
        if (movementBounds)
        {
            if (Mathf.Abs(transform.position.x) >= 95)
            {
                float direction = 1 / Mathf.Abs(transform.position.x) * transform.position.x;
                transform.position -= new Vector3(3 * direction, 0, 0);
            } 

            if (Mathf.Abs(transform.position.z) >= 95)
            {
                float direction = 1 / Mathf.Abs(transform.position.z) * transform.position.z;
                transform.position -= new Vector3(0, 0, 3 * direction);
            } 
        }
    }
}
