using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    public bool isFollowingPlayer = true;
    public float offsetRotX = 35f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isFollowingPlayer)
        {
            transform.position = player.position + offset;
            transform.rotation = Quaternion.Euler(offsetRotX, player.rotation.y, player.rotation.z);
        }
    }
}
