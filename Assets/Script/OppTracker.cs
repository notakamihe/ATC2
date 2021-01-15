using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OppTracker : MonoBehaviour
{
    public OppSpawner oppSpawner;
    public Text oppDeathCountCurrent;
    public Text oppCount;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        oppDeathCountCurrent.text = oppSpawner.numDeadOpps.ToString();
        oppCount.text = oppSpawner.numOppsMax.ToString();
    }
}
