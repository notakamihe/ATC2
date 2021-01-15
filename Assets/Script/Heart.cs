using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class Heart : MonoBehaviour
{
    public Image heartPrefab;
    NewPlayer player;

    List<Image> hearts = new List<Image>();

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<NewPlayer>();
        SpawnHearts();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Transform child in GetComponentsInChildren<Transform>().Skip(1))
            child.gameObject.GetComponent<RectTransform>().localEulerAngles = Vector3.zero;

        if (player.lives != hearts.Count)
        {
            print("Spawn");
            SpawnHearts();
        }
    }

    void SpawnHearts ()
    {
        hearts = new List<Image>(player.lives);

        foreach (Transform child in GetComponentsInChildren<Transform>().Skip(1))
            Destroy(child.gameObject);

        for (int i = 0; i < player.lives; i++)
        {
            hearts.Add(Instantiate(heartPrefab, transform.position - transform.right * (0.9f * i - 0.9f),
                Quaternion.identity, transform));
        }
    }
}