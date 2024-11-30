using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    public Sprite OnPower;
    public Sprite OnPowerOff;
    public AudioSource audio;
    public Image button;

    private void Start()
    {
        audio = GetComponent<AudioSource>();
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
}
