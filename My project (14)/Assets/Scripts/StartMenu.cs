using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    [SerializeField] Sprite OnPower;
    [SerializeField] Sprite OnPowerOff;
    [SerializeField] AudioSource audio;
    [SerializeField] Image button;

    private void Start()
    {
        //audio = gameObject.GetComponent<AudioSource>();
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
