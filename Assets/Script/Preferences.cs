using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Preferences : MonoBehaviour
{
    public static bool IsDashKinetic
    {
        get
        {
            return PlayerPrefs.GetInt("isDashKinetic", 1) == 1;
        }
        set
        {
            PlayerPrefs.SetInt("isDashKinetic", Convert.ToInt16(value));
        }
    }
}