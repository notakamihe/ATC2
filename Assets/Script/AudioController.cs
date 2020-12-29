using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioManager audioMgr;
    public GameControl gameCtrlr;
    public Sound lvlMusic;
    public Sound winMusic;
    public Sound loseMusic;
    public float lvlMusicVol;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (audioMgr == null)
        {
            InitializeAudMgrVars();
        }

        if (gameCtrlr.player.isDead)
        {
            Pause(lvlMusic);
        }

        if (gameCtrlr.gameWon)
        {
            Stop(lvlMusic);
        }

        if (gameCtrlr.gameWonCnvs.activeSelf)
        {
            Play(winMusic);
        } else if (gameCtrlr.gameOverCnvs.activeSelf)
        {
            Play(loseMusic);
        }
        
        if (gameCtrlr.isPaused)
        {
            Pause(lvlMusic);
        } else
        {
            if (!gameCtrlr.player.isDead)
            {
                UnPause(lvlMusic);
            }
        }
    }

    void InitializeAudMgrVars ()
    {
        audioMgr = gameCtrlr.audioManager;
        lvlMusic = audioMgr.GetSound(audioMgr.startMusicName);
        lvlMusicVol = lvlMusic.source.volume;
        winMusic = audioMgr.GetSound("Fes");
        loseMusic = audioMgr.GetSound("Moments");
    }

    void Pause (Sound sound, float delay = 0)
    {
        if (sound.source.isPlaying)
        {
            StartCoroutine(audioMgr.Pause(sound, delay));
        }
    }

    void Play (Sound sound, float delay = 0)
    {
        if (!sound.source.isPlaying)
        {
            StartCoroutine(audioMgr.Play(sound, delay));
        }
    }

    public void PlayButtonClick ()
    {
        if (audioMgr != null)
        {
            audioMgr.PlayButtonClick();
        }
    }

    void Stop (Sound sound, float delay = 0)
    {
        if (sound.source.isPlaying)
        {
            StartCoroutine(audioMgr.Stop(sound, delay));
        }
    }

    public void ToggleMusic ()
    {
        if (lvlMusic.source.volume > 0)
        {
            lvlMusic.source.volume = 0;
        } else
        {
            lvlMusic.source.volume = lvlMusicVol;
        }
    }

    void UnPause (Sound sound, float delay = 0)
    {
        if (!sound.source.isPlaying)
        {
            StartCoroutine(audioMgr.UnPause(sound, delay));
        }
    }
}
