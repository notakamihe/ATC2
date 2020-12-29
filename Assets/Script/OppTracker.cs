using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OppTracker : MonoBehaviour
{
    public GameControl gameController;
    public Text oppDeathCountCurrent;
    public Text oppCount;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        oppDeathCountCurrent.text = gameController.oppsDead.ToString();
        oppCount.text = gameController.oppCountLimit.ToString();
    }
}
