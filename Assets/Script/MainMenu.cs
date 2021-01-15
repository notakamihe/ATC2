using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject intro;
    public GameObject levelSelect;
    public GameObject beforeYouPlay;
    public GameObject settings;

    // Start is called before the first frame update
    void Start()
    {
        intro.SetActive(true);
        levelSelect.SetActive(false);
        beforeYouPlay.SetActive(false);
        settings.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void EnableBeforeYouPlay ()
    {
        intro.SetActive(!intro.activeSelf);
        beforeYouPlay.SetActive(!beforeYouPlay.activeSelf);
    }

    public void EnableLevelSelect ()
    {
        intro.SetActive(!intro.activeSelf);
        levelSelect.SetActive(!levelSelect.activeSelf);
    }

    public void ToggleSettings()
    {
        intro.SetActive(!intro.activeSelf);
        settings.SetActive(!settings.activeSelf);
    }

    public void RollCredits ()
    {
        SceneManager.LoadScene("Credits");
    }

    public void StartGame ()
    {
        SceneManager.LoadScene("Level1");
    }
}
