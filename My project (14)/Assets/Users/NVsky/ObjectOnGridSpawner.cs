using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectOnGridSpawner : MonoBehaviour
{
    public static ObjectOnGridSpawner Instance { get; private set; }
    [SerializeField] private List<GameObject> gridSpawnObjectPrefabs; // ������ �������� ��� ������
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private List<Image> uiIcons; // ������ UI-������ ��� ����������
    [SerializeField] private List<int> Costs; // ������ ���

    private int selectedIndex = 0; // ������ �������� ���������� �������
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
