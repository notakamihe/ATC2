using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class NewAmbusher : NewOpp, IJumper
{
    public float ambushRadius = 3.0f;
    AmbushState state;

    enum AmbushState { Dormant, Active }

    private void Start()
    {
        base.Start();
    }

    private void FixedUpdate()
    {
        if (isDead)
        {
            Die();
        }

        switch (state)
        {
            case AmbushState.Dormant:
                foreach (Canvas hud in GetComponentsInChildren<Canvas>())
                    hud.gameObject.SetActive(false);

                break;
            case AmbushState.Active:
                Look(player.transform.position);
                base.Approach();
                ShowUI();
                break;
        }

        if (PlayerInRange(ambushRadius) && !player.isDead)
        {
            Ambush();
        } else if (!PlayerInRange(playerDetectionRadius))
        {
            Disengage();
        }
    }

    void Ambush ()
    {
        state = AmbushState.Active;
    }

    public virtual void DetectForJump()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 8.0f, LayerMask.GetMask("Obstacle")))
        {
            if (Vector3.Angle(transform.forward, rb.velocity) <= 25 && rb.velocity.magnitude > 5.0f)
            {
                Jump(jumpForce);
            }
        }
    }

    void Disengage ()
    {
        state = AmbushState.Dormant;
    }

    public virtual void Jump(float force)
    {
        if (grounded)
        {
            rb.AddForce(0, force * Time.deltaTime, 0, ForceMode.VelocityChange);
        }
    }

    void ShowUI ()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).TryGetComponent(out Canvas canvas))
                canvas.gameObject.SetActive(true);
        }
    }
}