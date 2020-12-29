using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mother : MonoBehaviour
{
    public Opp opp;
    public Player player;
    public GameObject child;
    public float detectRangeBirth = 5f;
    public int spawnDistFrmMom = -2;
    public int kidCount = 3;
    public bool birthed = false;

    // Start is called before the first frame update
    void Start ()
    {
        
    }

    // Update is called once per frame
    void Update ()
    {
        player = opp.gameObject.GetComponent<Opp>().player;

        if (!opp.isDead)
        {
            Birth();
        }
    }

    void Birth ()
    {
        if (Vector3.Distance(player.transform.position, transform.position) <= detectRangeBirth && !birthed)
        {
            birthed = true;

            for (int i = 0; i < kidCount; i++)
            {
                GameObject kid = Instantiate(
                    child, 
                    new Vector3(
                        i-1+transform.position.x, 
                        transform.position.y, 
                        (transform.position + transform.forward * spawnDistFrmMom).z
                    ), 
                    Quaternion.identity
                );
                kid.GetComponent<Child>().mother = this.gameObject.GetComponent<Mother>();
            }
        }
    }
}
