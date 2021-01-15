using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProgress : MonoBehaviour
{
    public static int HighestLevelReached
    {
        get
        {
            return PlayerPrefs.GetInt("highestLevelReached", 1);
        }
        set
        {
            PlayerPrefs.SetInt("highestLevelReached", value);
        }
    }

    public static int LastLevelPlayed
    {
        get
        {
            return PlayerPrefs.GetInt("lastLevelPlayed", 1);
        }
        set
        {
            PlayerPrefs.SetInt("lastLevelPlayed", value);
        }
    }
}
