using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangePreferences : MonoBehaviour
{
    public Dropdown dashModeDropdown;

    // Start is called before the first frame update
    void Start()
    {
        dashModeDropdown.value = Preferences.IsDashKinetic ? 0 : 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleIsDashKinetic (int val)
    {
        Preferences.IsDashKinetic = val == 0;
    }

    public void ClearGameProgression ()
    {
        PlayerPrefs.DeleteAll();
    }
}
