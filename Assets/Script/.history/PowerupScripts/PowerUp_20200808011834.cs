using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public Player player;
    public float direction = 1;
    public float duration = 20f;
    public bool initialized = false;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        Float();
    }


    void Float ()
    {
        transform.position += new Vector3(0, 0.035f * direction, 0);

        if (transform.position.y > 1)
        {
            direction = -1;
        } 

        if (transform.position.y <= 0)
        {
            direction = 1;
        }
    }
}
