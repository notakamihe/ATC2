using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SFXToggle : MonoBehaviour
{
    public Button button;
    public EffectsController fxController;

    // Update is called once per frame
    void Update()
    {
        if (!fxController.sfxOn)
        {
            ColorBlock cb = button.colors;
            cb.normalColor = Color.grey;
            cb.selectedColor = Color.grey;
            button.colors = cb;
        } else
        {
            ColorBlock cb = button.colors;
            cb.normalColor = Color.white;
            cb.selectedColor = new Color32(255, 188, 188, 255);
            button.colors = cb;
        
        }
    }
}
