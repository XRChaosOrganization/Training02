using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIManagerComponent : MonoBehaviour
{
    public static UIManagerComponent uIm;
    public Animator transition;
    public GameObject pausePanel;
    public TextMeshProUGUI timerDisplayText;
    public AudioSource mainMenuSource;
    public GameObject endGamePanel;
    public TextMeshProUGUI p1HeightText;
    public TextMeshProUGUI p2HeightText;
    public TextMeshProUGUI winnerDisplayText;
    
    
    public void Awake()
    {
        uIm = this;
    }

    public void Update()
    {
        if (SceneManager.GetActiveScene().name == "Scene_Title") return; 

        float currentTime = GameManager.gm.timer;
        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);
        if (seconds<10)
        {
            timerDisplayText.text = minutes + ":0 " + seconds;
        }
        else
        {
            timerDisplayText.text = minutes + ":" + seconds;
        }
        
    }

    public void Play()
    {
        mainMenuSource.Play();
        StartCoroutine(LoadScene("Scene_Game"));
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Pause()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }

    public void ToMain()
    {
        StartCoroutine(LoadScene("Scene_Title"));
    }

    IEnumerator LoadScene(string _scene)
    {
        transition.SetTrigger("StartFadeOut");
        if (Time.timeScale != 1.0f)
            Time.timeScale = 1;
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene(_scene);
    }
    
    public void EndgameDisplay(int _P1BeanHeight, int _P2BeanHeight)
    {
        Time.timeScale = 0;
        timerDisplayText.gameObject.SetActive(true);
        endGamePanel.SetActive(true);
        p1HeightText.text = _P1BeanHeight.ToString() + " m";
        p2HeightText.text = _P2BeanHeight.ToString() + " m";
       
        if (_P1BeanHeight > _P2BeanHeight)
        {
            winnerDisplayText.text = "Player 1 wins !!";
        }
        else if (_P1BeanHeight<_P2BeanHeight)
        {
            winnerDisplayText.text = "Player 2 wins !!";
        }
        else
        {
            winnerDisplayText.text = "Draw Game !!";
        }
        
    }
}
