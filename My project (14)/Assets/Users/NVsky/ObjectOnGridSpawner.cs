using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectOnGridSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> gridSpawnObjectPrefabs;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private List<Image> uiIcons;

    private int selectedIndex = 0;

    private void Start()
    {
        UpdateUIIcons();
    }

    private void Update()
    {
        HandleInput();
    }
    private void HandleInput()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f)
        {
            ChangeSelectedIndex(-1);
        }
        else if (scroll < 0f)
        {
            ChangeSelectedIndex(1);
        }

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
            SpawnObject();
        }
    }

    private void ChangeSelectedIndex(int direction)
    {
        selectedIndex = (selectedIndex + direction + gridSpawnObjectPrefabs.Count) % gridSpawnObjectPrefabs.Count;
        Debug.Log($"Selected object: {gridSpawnObjectPrefabs[selectedIndex].name}");
        UpdateUIIcons();
    }

    private void SpawnObject()
    {
        if (gridSpawnObjectPrefabs.Count == 0) return;

        GameObject prefab = gridSpawnObjectPrefabs[selectedIndex];
        Vector3 spawnPosition = new Vector3(
            Mathf.CeilToInt(spawnPoint.position.x) / 2 * 2,
            0,
            Mathf.CeilToInt(spawnPoint.position.z) / 2 * 2
        );

        if (prefab.GetComponent<ObjectPlacer>() != null)
        {
            prefab.GetComponent<ObjectPlacer>().isSpawn = true;
        }
        else
        {
            prefab.transform.GetChild(0).GetComponent<ObjectPlacer>().isSpawn = true;
        }
        Instantiate(prefab, spawnPosition, Quaternion.identity);
        Debug.Log($"Spawned: {prefab.name}");
    }
    private void UpdateUIIcons()
    {
        for (int i = 0; i < uiIcons.Count; i++)
        {
            if (i == selectedIndex)
            {
                SetIconAlpha(uiIcons[i], 1.0f);
            }
            else
            {
                SetIconAlpha(uiIcons[i], 0.5f);
            }
        }
    }

    private void SetIconAlpha(Image icon, float alpha)
    {
        Color color = icon.color;
        color.a = alpha;
        icon.color = color;
    }
}
