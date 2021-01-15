using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public AudioSource audioSrc;

    public NewPlayer player;

    public float duration = 20f;

    // Start is called before the first frame update
    protected void Start()
    {
        player = Singleton.instance.player;
        audioSrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    protected void Update()
    {
        transform.Translate(Vector3.up * (Mathf.Cos(Time.time * 15.0f / Mathf.PI) * 1.0f * Time.deltaTime));
    }

    protected void OnTriggerEnter ()
    {
        AudioSource.PlayClipAtPoint(audioSrc.clip, transform.position, .5f);
    }
}