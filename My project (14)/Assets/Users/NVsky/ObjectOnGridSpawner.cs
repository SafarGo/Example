using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectOnGridSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> gridSpawnObjectPrefabs; // ������ �������� ��� ������
    [SerializeField] private Transform spawnPoint;

    private int selectedIndex = 0; // ������ �������� ���������� �������

    private void Update()
    {
        HandleInput();
    }

    /// <summary>
    /// ������������ ���� �� ������������.
    /// </summary>
    private void HandleInput()
    {
        // ������������ � ������� ������ ����
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f)
        {
            ChangeSelectedIndex(1);
        }
        else if (scroll < 0f)
        {
            ChangeSelectedIndex(-1);
        }

        // ������������ � ������� ���� �� ���������� (1-9)
        for (int i = 0; i < Mathf.Min(gridSpawnObjectPrefabs.Count, 9); i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                selectedIndex = i;
                Debug.Log($"Selected object: {gridSpawnObjectPrefabs[selectedIndex].name}");
            }
        }

        // ����� �������
        if (Input.GetKeyDown(KeyCode.T))
        {
            SpawnObject();
        }
    }

    /// <summary>
    /// ������ ������� ��������� ������.
    /// </summary>
    /// <param name="direction">����������� ��������� (1 ��� -1).</param>
    private void ChangeSelectedIndex(int direction)
    {
        selectedIndex = (selectedIndex + direction + gridSpawnObjectPrefabs.Count) % gridSpawnObjectPrefabs.Count;
        Debug.Log($"Selected object: {gridSpawnObjectPrefabs[selectedIndex].name}");
    }

    /// <summary>
    /// ������� ������ �� �������� ���������� �������.
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
