using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Random=UnityEngine.Random;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
    public Player player;
    public int randNum;
    public int oppCountSimult = 0;
    public int oppCountSimultLimit = 3;
    public int oppCount = 0;
    public int oppCountLimit = 12;
    public int oppsDead = 0;
    public bool isPaused = false;
    public Image pausePlayImg;
    public Button pausePlayButton;
    public bool pauseResumeCalled = false;
    public Sprite playSprite;
    public Sprite pauseSprite;
    public GameObject[] powerUps;
    public float startTime;
    public float powerUpSpawnInterval = 30;
    public int powerUpCount = 0;
    public int maxPowerUpCount = 5;
    public int bossCount = 0;
    public int bossCountDead = 0;
    public int nextLevel;
    public bool gameOver = false;
    public bool gameWon = false;
    public bool bossLevel = false;
    public bool settingsActive = false;
    public bool musicOn = true;
    public string confirmAction = "GoToMainMenu";
    public GameObject gameWonCnvs;
    public GameObject gameOverCnvs;
    public GameObject[] oppsArray;
    public GameObject settings;
    public GameObject confirm;
    public AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        gameWonCnvs.SetActive(false);
        gameOverCnvs.SetActive(false);
        settings.SetActive(false);
        isPaused = false;
        pauseSprite = pausePlayImg.sprite;
        nextLevel = SceneManager.GetActiveScene().buildIndex + 1;
        audioManager = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver)
        {
            StartCoroutine(LoseLevel(2));
        }

        if (gameWon)
        {
            StartCoroutine(WinLevel(2));
        }

        if ((oppsDead >= oppCountLimit && !bossLevel) || (bossCountDead >= bossCount && bossLevel))
        {
            gameWon = true;
        }

        settings.SetActive(isPaused);
        SpawnOpps();
        PausePlayMechanics();
        SpawnPowerups();
    }  

    void PausePlayMechanics ()
    {
        if (isPaused)
        {
            pausePlayButton.onClick.AddListener(Resume);
        } else
        {
            pausePlayButton.onClick.AddListener(Pause);
            confirm.SetActive(false);
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            DeterminePausePlay();
        }
    }

    void DeterminePausePlay()
    {   if (!Input.GetKey(KeyCode.Space))
        {
            if (isPaused)
            {
                Resume();
            } else
            {
                Pause();
            }
        }
    } 

    void Pause ()
    {
        if (!Input.GetKey(KeyCode.Space))
        {
            pausePlayImg.sprite = playSprite;
            isPaused = true;
            Time.timeScale = 0;
        }
    }

    void Resume ()
    {
        if (!Input.GetKey(KeyCode.Space))
        {
            pausePlayImg.sprite = pauseSprite;
            isPaused = false;
            Time.timeScale = 1;
        }
    }

    void SpawnOpps ()
    {
        if (oppCount < oppCountLimit && oppCountSimult < oppCountSimultLimit)
        {
            randNum = Random.Range(1, 100);

            if (randNum == 1)
            {
                float iteration = 0;
                bool goodPositionFound = false;
                Vector3 randPos = new Vector3(Random.Range(-90, 90), 0, Random.Range(-90, 90));

                while (iteration < 50)
                {
                    iteration++;
                    Collider[] colliders = Physics.OverlapSphere(randPos, 5);

                    if (colliders.Length <= 1)
                    {
                        goodPositionFound = true;
                        break;
                    }

                    randPos = new Vector3(Random.Range(-90, 90), 0, Random.Range(-90, 90));
                }

                if (goodPositionFound)
                {
                    oppCountSimult++;
                    oppCount++;
                    GameObject opp = oppsArray[Random.Range(0, oppsArray.Length)];
                    randPos.y = opp.GetComponent<Opp>().spawnY;
                    Instantiate(opp, randPos, Quaternion.identity);
                }
            }
        }
    }

    void SpawnPowerups ()
    {
        if (Time.timeSinceLevelLoad % powerUpSpawnInterval > powerUpSpawnInterval - .03 && powerUpCount < maxPowerUpCount)
        {
            int iteration = 0;
            bool goodSpawnPt = false;
            Vector3 randPos = new Vector3(Random.Range(-90, 90), 0, Random.Range(-90, 90));
            GameObject[] opps = GameObject.FindGameObjectsWithTag("Opps");

            while (iteration < 50)
            {
                iteration++;

                Collider[] colliders = Physics.OverlapSphere(randPos, 2);
                
                if (colliders.Length <= 1)
                {
                    goodSpawnPt = true;
                    break;
                }
            }

            if (goodSpawnPt)
            {
                powerUpCount++;
                Instantiate(powerUps[Random.Range(0, powerUps.Length)], randPos, Quaternion.identity);
            }
        }
    }

    public void LoadNextLevel ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void RestartLevel ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void GoToMainMenu ()
    {
        SceneManager.LoadScene(0);
    }

    public void SetConfirm (string action)
    {
        confirmAction = action;
    }

    public void Confirm ()
    {
        Invoke(confirmAction, 0);
        Resume();
    }


    IEnumerator WinLevel (float delay) 
    {
        yield return new WaitForSeconds(delay);
        gameWonCnvs.SetActive(true);

        if (nextLevel > PlayerPrefs.GetInt("levelLimit"))
        {
            PlayerPrefs.SetInt("levelLimit", nextLevel);
        }
    }

    IEnumerator LoseLevel (float delay)
    {
        yield return new WaitForSeconds(delay);
        gameOverCnvs.SetActive(true);
    }
}
