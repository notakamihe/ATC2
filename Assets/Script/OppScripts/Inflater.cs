using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Inflater : NewOpp, IJumper
{
    public AudioSource inflateAudio;
    public float inflateRadius = 3.0f;
    public float maxSizeMagnitude = 5.5f;
    public float growthFactor = 1.1f;

    protected void Start()
    {
        base.Start();
    }

    protected void FixedUpdate()
    {
        base.FixedUpdate();

        inflateAudio = GetComponent<AudioSource>();
        DetectForJump();
    }

    protected override void Approach()
    {
        base.Approach();

        if (PlayerInRange(inflateRadius) && transform.localScale.magnitude <= maxSizeMagnitude)
        {
            Inflate();

            if (!inflateAudio.isPlaying)
                inflateAudio.Play();
        }
        else
            if (inflateAudio.isPlaying)
                inflateAudio.Stop();
    }

    public void DetectForJump()
    {
        Debug.DrawRay(transform.position - Vector3.up * (transform.localScale.z * 0.4f), transform.forward * 16, Color.white);

        if (Physics.Raycast(transform.position - Vector3.up * (transform.localScale.z * 0.4f), 
            transform.forward, out RaycastHit hit, 8.0f, LayerMask.GetMask("Obstacle")))
        {

            if (Vector3.Angle(transform.forward, rb.velocity) <= 25 && rb.velocity.magnitude > 5.0f)
                Jump(jumpForce);
        }
    }

    void Inflate ()
    {
        transform.localScale *= growthFactor;
        rb.mass *= growthFactor;
        speed *= growthFactor;
        defenseModifier = Mathf.Clamp(defenseModifier / growthFactor, 0.5f, 2.0f);
        meleeDamage = Mathf.Clamp(meleeDamage * growthFactor, 5.0f, 40.0f);
        knockbackForce *= growthFactor;
        playerDetectionRadius = Mathf.Clamp(playerDetectionRadius * growthFactor, 10.0f, 25.0f);

        maxHealth = Mathf.Clamp(maxHealth * growthFactor, 20.0f, 150.0f);
        health = maxHealth;
    }

    public void Jump(float force)
    {
        if (grounded)
        {
            rb.AddForce(0, force * Time.deltaTime, 0, ForceMode.VelocityChange);
        }
    }
}