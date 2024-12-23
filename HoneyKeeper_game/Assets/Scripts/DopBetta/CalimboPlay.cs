using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalimboPlay : MonoBehaviour
{
    public List <AudioClip> SpawnedSound; // Массив объектов для спауна
    public GameObject SpawnedNote;
    public Transform spawnPoint; // Точка спауна
    AudioSource _spawnedNote;

    private KeyCode[] spawnKeys = {
        KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R, KeyCode.T, KeyCode.Y,
        KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.F, KeyCode.G, KeyCode.H,
        KeyCode.Z, KeyCode.X, KeyCode.C, KeyCode.V, KeyCode.B
    };

    void Update()
    {
        // Пройдем по всем клавишам и спауним объект, если клавиша была нажата
        for (int i = 0; i < spawnKeys.Length; i++)
        {
            if (Input.GetKeyDown(spawnKeys[i]))
            {
                SpawnObject(i + 1);
                break;
            }
        }
    }

    void SpawnObject(int objectNumber)
    {
        // Проверяем, чтобы индекс объекта был в пределах массива
        if (objectNumber >= 1 && objectNumber <= SpawnedSound.Count)
        {
            _spawnedNote =  Instantiate(SpawnedNote, spawnPoint.position, Quaternion.identity).GetComponent<AudioSource>();
            _spawnedNote.clip = SpawnedSound;
        }
        else
        {
            Debug.LogWarning("Неверный номер объекта");
        }
    }
}
