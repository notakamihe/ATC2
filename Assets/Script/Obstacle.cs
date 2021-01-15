using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= -3.0f)
        {
            Destroy(this.gameObject);
        }
    }
}
