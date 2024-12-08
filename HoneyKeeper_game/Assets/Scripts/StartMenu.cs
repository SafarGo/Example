using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public Sprite OnPower;
    public Sprite OnPowerOff;
    public AudioSource audio;
    public Image button;

    private void Start()
    {

    }

    public void OnPointerEnter()
    {
        button.sprite = OnPower;
        audio.Play();
    }

    public void OnPointerExit()
    {
        button.sprite = OnPowerOff;
    }

    public void Dialog()
    {
        StaticHolder.isFirstGame = true;
        JsonSaver._instance.DelateSavings();
        Debug.Log(StaticHolder.isFirstGame);
        SceneManager.LoadScene("UICanvas");

    }

    public void Exit()
    {
        SceneManager.LoadScene("StarttScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ContinueTheGame()
    {
        Debug.Log(StaticHolder.isFirstGame);
        SceneManager.LoadScene("SampleScene");
    }
}
