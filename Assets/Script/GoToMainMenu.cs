using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToMainMenu : MonoBehaviour
{
    public void MainMenu ()
    {
        SceneManager.LoadScene(0);
    }
}
