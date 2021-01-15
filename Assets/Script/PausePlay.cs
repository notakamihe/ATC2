using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class PausePlay : MonoBehaviour
{
    bool isPaused = false;

    public Sprite playSprite;
    public Sprite pauseSprite;
    public Image image;

    public GameObject pauseMenu;
    public Button[] confirmButtons;

    public void TogglePause ()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0.0f;
            pauseMenu.SetActive(true);
            image.sprite = playSprite;
            Singleton.instance.audioManager.music.Pause();
        }
        else
        {
            Time.timeScale = 1.0f;
            pauseMenu.SetActive(false);
            image.sprite = pauseSprite;
            Singleton.instance.audioManager.music.UnPause();

            DeactivateOtherConfirms(null);
        }
    }

    public void DeactivateOtherConfirms (Button btn)
    {
        foreach (Button button in confirmButtons)
        {
            if (button != btn)
                button.gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        Time.timeScale = 1.0f;
    }
}