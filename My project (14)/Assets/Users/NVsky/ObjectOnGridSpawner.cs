using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectOnGridSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> gridSpawnObjectPrefabs; // ������ �������� ��� ������
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private List<Image> uiIcons; // ������ UI-������ ��� ����������
    [SerializeField] private List<int> Costs; // ������ ���

    private int selectedIndex = 0; // ������ �������� ���������� �������

    private void Start()
    {
        UpdateUIIcons(); // ��������� ������ ��� ������
    }

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
            ChangeSelectedIndex(-1);
        }
        else if (scroll < 0f)
        {
            ChangeSelectedIndex(1);
        }

        // ������������ � ������� ���� �� ���������� (1-9)
        for (int i = 0; i < Mathf.Min(gridSpawnObjectPrefabs.Count, 9); i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                selectedIndex = i;
                Debug.Log($"Selected object: {gridSpawnObjectPrefabs[selectedIndex].name}");
                UpdateUIIcons();
            }
        }

        // ����� �������
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (StaticHolder.count_of_simple_honey >= Costs[selectedIndex])
            {
                ReservoirController.instance.currentHuneyCount -= Costs[selectedIndex];// ���� �������� �������� ��� ���� �����������
                StaticHolder.count_of_simple_honey -= Costs[selectedIndex];
                ReservoirController.instance.UpdateHoneyText();
                SpawnObject();
            }
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
        UpdateUIIcons();
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

        ///////����� ������ ����if (prefab.GetComponent<ObjectPlacer>() != null)
        ///////����� ������ ����{
        ///////����� ������ ����    prefab.GetComponent<ObjectPlacer>().isSpawn = true;
        ///////����� ������ ����}
        ///////����� ������ ����else
        ///////����� ������ ����{
        ///////����� ������ ����    prefab.transform.GetChild(0).GetComponent<ObjectPlacer>().isSpawn = true;
        ///////����� ������ ����}
       GameObject addedObject = Instantiate(prefab, spawnPosition, Quaternion.identity);
         StaticHolder.AllSpawnedObjects.Add(addedObject);
        JsonSaver._instance.Save();
        //Debug.Log($"Spawned: {prefab.name}");
       // Debug.Log(StaticHolder.AllSpawnedObjects[0]);
    }

    /// <summary>
    /// ��������� UI-������ � ����������� �� ���������� �������.
    /// </summary>
    private void UpdateUIIcons()
    {
        for (int i = 0; i < uiIcons.Count; i++)
        {
            if (i == selectedIndex)
            {
                SetIconAlpha(uiIcons[i], 1.0f); // ������ ��������� ��������� ������
            }
            else
            {
                SetIconAlpha(uiIcons[i], 0.5f); // ���������� ���������
            }
        }
    }

    /// <summary>
    /// ������������� ������������ ��� ��������� ������.
    /// </summary>
    /// <param name="icon">������ ��� ���������.</param>
    /// <param name="alpha">������������ (�� 0 �� 1).</param>
    private void SetIconAlpha(Image icon, float alpha)
    {
        Color color = icon.color;
        color.a = alpha;
        icon.color = color;
    }
}
