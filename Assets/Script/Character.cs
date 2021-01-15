using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [HideInInspector] public Rigidbody rb;
    public Animation flipAnim;
    [SerializeField] protected Transform animParent;

    public float maxHealth = 100.0f;
    [HideInInspector] public float health;

    public float speed = 2000.0f;
    public float rotSpeed = 250.0f;
    public float jumpForce = 750.0f;
    public float flipForce = 1000.0f;

    public float meleeDamage = 20.0f;
    public float defenseModifier = 1.0f;

    public bool isDead = false;
    protected bool shouldFlip = true;
    protected bool grounded = false;

    // Start is called before the first frame update
    protected void Start()
    {
        health = maxHealth;

        rb = GetComponent<Rigidbody>();
        flipAnim = GetComponent<Animation>();
    }

    // Update is called once per frame
    protected void Update()
    {
        if (health <= 0)
        {
            isDead = true;
        }

        if (transform.position.y <= -3)
        {
            isDead = true;
            shouldFlip = false;
        }
        else
            shouldFlip = true;
    }

    protected virtual void Die()
    {
    }

    protected void Flip ()
    {
        for (int i = 0; i < 10; i++)
        {
            rb.AddForce(Vector3.up * flipForce * Time.deltaTime, ForceMode.Acceleration);
        }

        animParent.SetParent(null);
        transform.SetParent(animParent);
        flipAnim.Play();
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            grounded = true;
        }
    }

    protected virtual void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            grounded = false;
        }
    }

    public virtual void TakeDamage (float dmg)
    {
        health = Mathf.Clamp(health - dmg, 0.0f, maxHealth);
    }
}
