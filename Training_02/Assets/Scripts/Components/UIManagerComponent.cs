using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManagerComponent : MonoBehaviour
{
    public Animator transition;
    public GameObject pausePanel;
    public static UIManagerComponent uIm;
    public void Awake()
    {
        uIm = this;
    }
    public void Play()
    {
        SceneManager.LoadScene("Scene_Game");
        StartCoroutine(FadeAnimation());
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
        Time.timeScale = 1;
        SceneManager.LoadScene("Scene_Title");
        StartCoroutine(FadeAnimation());
    }

    IEnumerator FadeAnimation()
    {
        transition.SetTrigger("StartFadeOut");
        yield return new WaitForSeconds(.8f);
    }
}
