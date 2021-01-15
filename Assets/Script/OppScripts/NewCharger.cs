using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class NewCharger : NewOpp, IJumper
{
    public AudioSource chargeAudio;
    public float chargeRate = 3.0f;

    float chargeRadius;
    float lastCharge;

    protected void Start()
    {
        base.Start();

        chargeRadius = playerDetectionRadius * 0.5f;
    }

    protected void FixedUpdate()
    {
        base.FixedUpdate();

        DetectForJump();

        if (PlayerInRange(chargeRadius) && Time.time > lastCharge + chargeRate)
        {
            Charge();
        }
    }

    void Charge ()
    {
        lastCharge = Time.time;
        rb.MovePosition(player.transform.position + player.transform.forward * 3.0f);
        PlayChargeSound();
    }

    public void DetectForJump()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 8.0f, LayerMask.GetMask("Obstacle")))
        {
            if (Vector3.Angle(transform.forward, rb.velocity) <= 25 && rb.velocity.magnitude > 5.0f)
            {
                Jump(jumpForce);
            }
        }
    }

    public void Jump(float force)
    {
        if (grounded)
        {
            rb.AddForce(0, force * Time.deltaTime, 0, ForceMode.VelocityChange);
        }
    }

    IEnumerator PlayChargeSound ()
    {
        chargeAudio.Play();

        while (true)
        {
            if (Time.time > lastCharge + 2.0f)
            {
                chargeAudio.Stop();
                break;
            }

            yield return null;
        }
    }
}