using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannonball : MonoBehaviour
{
    public GameObject explosionPrefab;
    public Rigidbody rb;
    public float lifetime = 3.0f;
    public float damage = 40.0f;

    float startTime;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;

        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > startTime + lifetime)
            Destruct();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destruct();
    }

    public void Destruct ()
    {
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(explosion, 3.0f);
        Destroy(this.gameObject);
    }
}
