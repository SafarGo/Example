using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectOnGridSpawner : MonoBehaviour
{
    public static ObjectOnGridSpawner Instance { get; private set; }
    [SerializeField] private List<GameObject> gridSpawnObjectPrefabs; // Список объектов для спауна
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private List<Image> uiIcons; // Список UI-иконок для затемнения
    [SerializeField] private List<int> Costs; // Список цен

    private int selectedIndex = 0; // Индекс текущего выбранного объекта
    int IDToSaveObjectTransforms = 0;

    private void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        JsonSaver._instance.Load();
        Debug.Log("LANANANANANNAAN" + StaticHolder.AllSpawnedObjectsID.Count);
        TestSpawn();

        UpdateUIIcons(); // Обновляем иконки при старте
    }

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
            ChangeSelectedIndex(-1);
        }
        else if (scroll < 0f)
        {
            ChangeSelectedIndex(1);
        }

        // Переключение с помощью цифр на клавиатуре (1-9)
        for (int i = 0; i < Mathf.Min(gridSpawnObjectPrefabs.Count, 9); i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                selectedIndex = i;
                Debug.Log($"Selected object: {gridSpawnObjectPrefabs[selectedIndex].name}");
                UpdateUIIcons();
            }
        }

        // Спавн объекта
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (StaticHolder.count_of_simple_honey >= Costs[selectedIndex])
            {
                ReservoirController.instance.currentHuneyCount -= Costs[selectedIndex];// надо доделать механику для двух резервуаров
                StaticHolder.count_of_simple_honey -= Costs[selectedIndex];
                ReservoirController.instance.UpdateHoneyText();
                SpawnObject();
            }
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
        UpdateUIIcons();
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


       GameObject _spawnedObject = Instantiate(prefab, spawnPosition, Quaternion.identity);
        StaticHolder.AllSpawnedObjectsID.Add(selectedIndex);
        StaticHolder.AllSpawnedObjectsTranforms.Add(_spawnedObject.transform.position);
        StaticHolder.AllSpawnedObjectsRotations.Add(_spawnedObject.transform.rotation);
        _spawnedObject.GetComponent<ObjectPlacer>().ObjectID = IDToSaveObjectTransforms;
        IDToSaveObjectTransforms++;
       // _spawnedObject.GetComponent<ObjectPlacer>().ObjectSpawnerID = selectedIndex;
        JsonSaver._instance.Save();
    }

    /// <summary>
    /// Обновляет UI-иконки в зависимости от выбранного индекса.
    /// </summary>
    private void UpdateUIIcons()
    {
        for (int i = 0; i < uiIcons.Count; i++)
        {
            if (i == selectedIndex)
            {
                SetIconAlpha(uiIcons[i], 1.0f); // Полная видимость выбранной иконки
            }
            else
            {
                SetIconAlpha(uiIcons[i], 0.5f); // Затемнение остальных
            }
        }
    }

    /// <summary>
    /// Устанавливает прозрачность для указанной иконки.
    /// </summary>
    /// <param name="icon">Иконка для изменения.</param>
    /// <param name="alpha">Прозрачность (от 0 до 1).</param>
    private void SetIconAlpha(Image icon, float alpha)
    {
        Color color = icon.color;
        color.a = alpha;
        icon.color = color;
    }

    void TestSpawn()
    {
        for (int i = 0; i < StaticHolder.AllSpawnedObjectsID.Count; i++)
        {
            if (StaticHolder.AllSpawnedObjectsID[i] != null)
            {
                GameObject startSpawnObject = Instantiate(gridSpawnObjectPrefabs[StaticHolder.AllSpawnedObjectsID[i]], StaticHolder.AllSpawnedObjectsTranforms[i], StaticHolder.AllSpawnedObjectsRotations[i]);
                IDToSaveObjectTransforms++;
                startSpawnObject.GetComponent<ObjectPlacer>().ObjectID = i;
            }
            else
            {
                Debug.LogError("NULL");

            }
        }
        Debug.Log("LLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL");
    }


}
