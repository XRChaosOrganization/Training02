using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManagerComponent : MonoBehaviour
{
    public static UIManagerComponent uIm;
    public Animator transition;
    public GameObject pausePanel;
    public TextMeshProUGUI timerDisplayText;
    public AudioSource mainMenuSource; 

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
        timerDisplayText.text = minutes + ":" + seconds;
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
}
