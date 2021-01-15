using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartContinueButton : MonoBehaviour
{
    public Text text;

    // Update is called once per frame
    void Update()
    {
        if (GameProgress.LastLevelPlayed == 1)
            text.text = "START";
        else
            text.text = "CONTINUE";
    }
}
