using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    RectTransform rectTransform;
    Character character;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        character = (Character)GetComponentInParent(typeof(Character));
    }

    // Update is called once per frame
    void Update()
    {
        float health = Mathf.InverseLerp(0.0f, character.maxHealth, character.health) * 100;
        rectTransform.sizeDelta = new Vector2(health, rectTransform.sizeDelta.y);
    }
}
