using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class NewChild : NewOpp, IJumper
{
    public NewMother mother;

    bool MotherFound
    {
        get
        {
            if (mother)
                return Vector3.Distance(mother.transform.position, transform.position) <= 3.0f;
            return false;
        }
    }

    protected void Update()
    {
        if (isDead)
        {
            Die();
        }

        if (PlayerInRange(playerDetectionRadius) && !player.isDead)
        {
            Look(player.transform.position);
            base.Approach();
        } else if (!MotherFound)
        {
            FindMother();
        }

        DetectForJump();
    }

    public void DetectForJump()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 8.0f,
            LayerMask.GetMask("Obstacle")))
        {
            if (Vector3.Angle(transform.forward, rb.velocity) <= 25 && rb.velocity.magnitude > 5.0f)
                Jump(jumpForce);
        }
    }

    void FindMother()
    {
        if (mother)
        {
            Look(mother.transform.position);
            Move();
        }
    }

    public void Jump(float force)
    {
        if (grounded)
        {
            rb.AddForce(0, force * Time.deltaTime, 0, ForceMode.VelocityChange);
        }
    }
}