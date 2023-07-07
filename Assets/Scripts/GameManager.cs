using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    private Boolean pause;
    private Text text;
    public Canvas canvas;
    public GameObject walls;

    private int playerHealth;
    public GameObject[] hearts;

    public Canvas pauseCanvas;
    public GameObject music;
    private bool isMuted;

    private PlayerBehavior player;

    public GameObject gameHistory;

    public GameObject abilities;
    public GameObject slashIcon;
    public GameObject wallIcon;
    public GameObject dashIcon;


    public void Awake()
    {
        isMuted = PlayerPrefs.GetInt("muted",0) == 1;
    } 

    // Start is called before the first frame update
    void Start()
    {
        text =  null;
        pause = false;
        player = GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<PlayerBehavior>();
        playerHealth = player.getHealth();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void winlevel()
    {
        Scene activeScene = SceneManager.GetActiveScene();

        // Si llegamos al ultimo nivel que desarrollamos, retorna al menu principal
        // Como solo tenemos 3 escenas hasta el momento, se retorna al reconocer que 
        // terminamos la escena en el indice #2
        if (activeScene.buildIndex >= 2) {
            SceneManager.LoadScene(0);
        }

        if (activeScene.buildIndex < SceneManager.sceneCountInBuildSettings - 1)
        {
            SceneManager.LoadScene(activeScene.buildIndex + 1);
        }
    }

    public void MutePressed()
    {
        isMuted = !isMuted;
        AudioListener.pause = isMuted; 
        PlayerPrefs.SetInt("muted", isMuted ? 1 : 0);
    }

    public void toggleHistory()
    {
        gameHistory.SetActive(!gameHistory.active);
    }
    
    public void goToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void showMessage()
    {
        canvas.gameObject.SetActive(true);
        text = canvas.GetComponent<UIHelper>().getText();

        player.Pause();
    }

    public void retryLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
  
    public void toggleWalls()
    {
        walls.SetActive(!walls.active);
    }

    public void updateLife()
    {
        playerHealth = player.getHealth();
        for(int i = 0; i < hearts.Length; i++)
        {
            if (playerHealth > i)
            {
                hearts[i].SetActive(true);
            } else
            {
                hearts[i].SetActive(false);
            }
        }
    }

    public void pauseGame()
    {
        if (pauseCanvas)
        {
            this.pause = !pause;

            if (!pause)
            {
                isMuted = false;
            }

            if (!pause && !isMuted)
            { 
                music.GetComponent<AudioSource>().Play();
            } else
            {
                music.GetComponent<AudioSource>().Pause();
            }

            pauseCanvas.gameObject.SetActive(!pauseCanvas.gameObject.active);
            Time.timeScale = pause ? 0 : 1;
            player.Pause();
        }
    }

    public void toggleTextIcon()
    {
        abilities.SetActive(true);
    }
    public void toggleDashIcon()
    {
        dashIcon.SetActive(true);
    }

    public void toggleSlashIcon()
    {
        slashIcon.SetActive(true);
    }

    public void toggleWJIcon()
    {
        wallIcon.SetActive(true);
    }
}
