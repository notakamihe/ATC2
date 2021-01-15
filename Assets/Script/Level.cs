using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Level : MonoBehaviour
{
    public GameObject gameOverScreen;
    public GameObject gameWonScreen;

    public LevelStatus status;

    public enum LevelStatus { Ongoing, Over, Won }

    private void Start()
    {
        gameOverScreen.SetActive(false);
        gameWonScreen.SetActive(false);

        GameProgress.LastLevelPlayed = SceneManager.GetActiveScene().buildIndex;
    }

    private void Update()
    {
        switch (status)
        {
            case LevelStatus.Ongoing:
                break;
            case LevelStatus.Over:
                StartCoroutine(LoseLevel());
                this.enabled = false;
                break;
            case LevelStatus.Won:
                StartCoroutine(WinLevel());
                this.enabled = false;
                break;
        }
    }

    IEnumerator WinLevel()
    {
        int nextLevel = SceneManager.GetActiveScene().buildIndex + 1;

        if (GameProgress.HighestLevelReached < nextLevel)
            GameProgress.HighestLevelReached = nextLevel;

        yield return new WaitForSeconds(2.0f);
        gameWonScreen.SetActive(true);
    }

    IEnumerator LoseLevel ()
    {
        yield return new WaitForSeconds(2.0f);
        gameOverScreen.SetActive(true);
    }
}