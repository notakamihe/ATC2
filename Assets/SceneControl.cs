using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{
    public Button[] lvlButtons;

    // Start is called before the first frame update
    void Start()
    {
        int levelLimit = PlayerPrefs.GetInt("levelLimit", 1);
        

        if (lvlButtons != null)
        {
            for (int i = 0; i < lvlButtons.Length; i++)
            {
                if (i + 1 > levelLimit)
                {
                    lvlButtons[i].interactable = false;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
