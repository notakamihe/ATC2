using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class NewMother : NewOpp, IJumper
{
    public NewChild childPrefab;
    public float birthRadius = 3.0f;
    public int numChildren = 3;

    bool hasBirthed = false;

    protected void Start()
    {
        base.Start();
    }

    protected void FixedUpdate()
    {
        base.FixedUpdate();

        DetectForJump();
    }

    protected override void Approach()
    {
        base.Approach();

        if (PlayerInRange(birthRadius) && !hasBirthed)
        {
            Birth();
        }
    }

    void Birth()
    {
        hasBirthed = true;

        for (int i = 0; i < numChildren; i++)
        {
            NewChild child = Instantiate(childPrefab, 
                (transform.position - transform.forward * 2) + transform.right * (i - 1), Quaternion.identity);
            child.mother = this;
        }
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

    public void Jump(float force)
    {
        if (grounded)
        {
            rb.AddForce(0, force * Time.deltaTime, 0, ForceMode.VelocityChange);
        }
    }
}