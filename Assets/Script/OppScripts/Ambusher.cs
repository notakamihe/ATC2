using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ambusher : MonoBehaviour
{
    public Opp opp;
    public Follower follower;
    public GameObject ptrUI;
    public GameObject healthUI;
    public float detectRangeAmbush = 3f;
    public bool stationary = true;
    
    // Start is called before the first frame update
    void Start()
    {
        follower.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        EnableDisableUI();
        Ambush();
    }

    void Ambush ()
    {
        if (opp.DistFrmPlayer() <= detectRangeAmbush)
        {
            stationary = false;
            follower.enabled = true;
        }

        if (opp.DistFrmPlayer() >= opp.detectionRadius * 2)
        {
            stationary = true;
            follower.enabled = false;
        }
    }

    void EnableDisableUI()
    {
        ptrUI.SetActive(!stationary);
        healthUI.SetActive(!stationary);
    }
}
