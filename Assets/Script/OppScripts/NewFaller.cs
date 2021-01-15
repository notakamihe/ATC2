using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class NewFaller : NewShooter
{
    bool hasFallen = false;

    protected void Start()
    {
        base.Start();

        Vector3 newPos = transform.position;
        newPos.y = 3.0f;
        transform.position = newPos;
    }

    protected void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void Approach()
    {
        if (!hasFallen)
        {
            Move();

            if (Physics.SphereCast(transform.position, 1.0f, Vector3.down, out RaycastHit hit))
            {
                if (hit.collider.gameObject == player.gameObject)
                    if (player.rb.velocity.magnitude <= 10.0f)
                        Fall();
            }
        } else
        {
            Shoot();
        }
    }

    public override void DetectForJump()
    {
    }

    void Fall ()
    {
        hasFallen = true;
        rb.useGravity = true;
        rb.drag = 0;
    }

    public override void Jump(float force)
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        print("Hit");
    }
}