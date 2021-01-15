using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = .15f;

    void Update ()
    {
        transform.position = transform.parent.position;
    }
}
