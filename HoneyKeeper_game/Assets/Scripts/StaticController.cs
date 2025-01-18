using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticController : MonoBehaviour
{
    public static StaticController instance;
    // Start is called before the first frame update
    [SerializeField] GameObject centerCursor;
    [SerializeField] GameObject centerCursor2;
    float constantMoveingSpeed;
    private List<(AudioSource, bool)> audioSourcesState = new List<(AudioSource, bool)>();
    void Start()
    {
        if (instance == null)
            instance = this;
        centerCursor = GameObject.Find("Center--DNS--");
        centerCursor2 = GameObject.Find("CrosshairAndStamina");
    }

    public void TurnCursorInState(bool state)
    {
        centerCursor.SetActive(state);
        centerCursor2.SetActive(state);
    }

    public void SetMoveingSpeed(GameObject objecttoChandgespeed,int speed, int sprintspeed)
    {
        objecttoChandgespeed.GetComponent<FirstPersonController>().walkSpeed = speed;
        objecttoChandgespeed.GetComponent<FirstPersonController>().sprintSpeed = sprintspeed;
    }

    public void MuteAllAudioSources()
    {
        audioSourcesState.Clear(); // Очищаем предыдущие состояния

        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audioSource in audioSources)
        {
            audioSourcesState.Add((audioSource, audioSource.mute)); // Сохраняем текущее состояние
            audioSource.mute = true; // Мутим
        }
    }

    // Метод для восстановления прежних состояний AudioSource
    public void RestoreAudioSources()
    {
        foreach (var (audioSource, wasMuted) in audioSourcesState)
        {
            if (audioSource != null) // Проверяем, что объект существует
            {
                audioSource.mute = wasMuted; // Восстанавливаем прежнее состояние
            }
        }

        audioSourcesState.Clear(); // Очищаем список после восстановления
    }

}
