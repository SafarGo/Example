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
        SceneManager.LoadScene("UICanvas");
    }

    public void Exit()
    {
        SceneManager.LoadScene("StarttScene");
    }
}
