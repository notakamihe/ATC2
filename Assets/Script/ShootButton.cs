using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShootButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public NewPlayer player;
    bool isBeingHeldDown = false;

    public void OnPointerDown (PointerEventData data)
    {
        isBeingHeldDown = true;
    }

    public void OnPointerUp (PointerEventData data)
    {
        isBeingHeldDown = false;
    }

    void Update ()
    {
        if (isBeingHeldDown)
            player.Shoot();
    }
}
