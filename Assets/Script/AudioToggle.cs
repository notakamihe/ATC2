using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioToggle : MonoBehaviour
{
    public AudioController audioController;
    public Button button;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
       if (audioController.lvlMusic.source.volume == 0)
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
