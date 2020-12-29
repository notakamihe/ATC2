using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject intro;
    public GameObject levelSelect;
    public GameObject controls;

    // Start is called before the first frame update
    void Start()
    {
        intro.SetActive(true);
        levelSelect.SetActive(false);
        controls.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void EnableControls ()
    {
        intro.SetActive(!intro.activeSelf);
        controls.SetActive(!controls.activeSelf);
    }

    public void EnableLevelSelect ()
    {
        intro.SetActive(!intro.activeSelf);
        levelSelect.SetActive(!levelSelect.activeSelf);
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
