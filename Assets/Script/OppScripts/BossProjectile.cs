using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    public GameObject explosion;
    public float projLifetime = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(Explode(projLifetime));
    }

    void OnCollisionEnter (Collision col)
    {
        if (col.gameObject.GetComponent<Boss>() == null)
        {
            StartCoroutine(Explode(0));
        }
    }

    public IEnumerator Explode (float delay)
    {
        yield return new WaitForSeconds(delay);
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
