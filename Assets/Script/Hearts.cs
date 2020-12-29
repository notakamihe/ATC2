using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hearts : MonoBehaviour
{
    public int heartCountMax;
    public int heartCount;
    public Image[] hearts;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
    
    public void RemoveHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {   
            if (i == heartCount)
            {
                Destroy(hearts[i]);
            }
        }
    }
}
