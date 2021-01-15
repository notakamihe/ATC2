using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    public bool isFollowingPlayer = true;

    // Update is called once per frame
    void Update()
    {
        if (isFollowingPlayer)
        {
            transform.localPosition = offset;
        }
    }
}
