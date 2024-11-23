using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectOnGridSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> gridSpawnObjectPrefabs; // Список объектов для спауна
    [SerializeField] private Transform spawnPoint;

    private int selectedIndex = 0; // Индекс текущего выбранного объекта

    private void Update()
    {
        HandleInput();
    }

    /// <summary>
    /// Обрабатывает ввод от пользователя.
    /// </summary>
    private void HandleInput()
    {
        // Переключение с помощью колеса мыши
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f)
        {
            ChangeSelectedIndex(1);
        }
        else if (scroll < 0f)
        {
            ChangeSelectedIndex(-1);
        }

        // Переключение с помощью цифр на клавиатуре (1-9)
        for (int i = 0; i < Mathf.Min(gridSpawnObjectPrefabs.Count, 9); i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                selectedIndex = i;
                Debug.Log($"Selected object: {gridSpawnObjectPrefabs[selectedIndex].name}");
            }
        }

        // Спавн объекта
        if (Input.GetKeyDown(KeyCode.T))
        {
            SpawnObject();
        }
    }

    /// <summary>
    /// Меняет текущий выбранный индекс.
    /// </summary>
    /// <param name="direction">Направление изменения (1 или -1).</param>
    private void ChangeSelectedIndex(int direction)
    {
        selectedIndex = (selectedIndex + direction + gridSpawnObjectPrefabs.Count) % gridSpawnObjectPrefabs.Count;
        Debug.Log($"Selected object: {gridSpawnObjectPrefabs[selectedIndex].name}");
    }

    /// <summary>
    /// Спавнит объект по текущему выбранному индексу.
    /// </summary>
    private void SpawnObject()
    {
        if (gridSpawnObjectPrefabs.Count == 0) return;

        GameObject prefab = gridSpawnObjectPrefabs[selectedIndex];
        Vector3 spawnPosition = new Vector3(
            Mathf.CeilToInt(spawnPoint.position.x) / 2 * 2,
            0,
            Mathf.CeilToInt(spawnPoint.position.z) / 2 * 2
        );

        Instantiate(prefab, spawnPosition, Quaternion.identity);
        Debug.Log($"Spawned: {prefab.name}");
    }
}
