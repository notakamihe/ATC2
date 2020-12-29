using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Child : MonoBehaviour
{
    public Opp opp;
    public Follower follower;
    public Mother mother;
    public float behindDistance = 2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FindMother();
    }

    void FindMother ()
    {
        if (!follower.playerFound && mother != null)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(mother.transform.position - transform.position),
                opp.rotSpeed * Time.deltaTime
            );
            
            if (Vector3.Distance(mother.transform.position, transform.position) >= behindDistance)
            {
                opp.rb.AddRelativeForce(0, 0, opp.speed * Time.deltaTime);
            }
        } 
    }
}
