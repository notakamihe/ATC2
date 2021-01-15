using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Scenes : MonoBehaviour
{
    public void RestartLevel ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RollCredits ()
    {
        SceneManager.LoadScene("Credits");
    }

    public void StartLastLevelPlayed ()
    {
        SceneManager.LoadScene(GameProgress.LastLevelPlayed);
    }

    public void StartNextLevel ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ToMainMenu ()
    {
        SceneManager.LoadScene(0);
    }
}