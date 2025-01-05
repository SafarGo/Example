using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;  // Добавлено для использования File

public class RockGenerator : MonoBehaviour
{
    [Header("Настройки деформации")]
    public float deformationIntensity = 0.5f; // Максимальная величина деформации
    public int noiseIterations = 3;          // Количество итераций шума для добавления реалистичности
    public float noiseScale = 0.3f;          // Масштаб шума

    [Header("Размер камня")]
    public Vector3 minScale = new Vector3(0.8f, 0.8f, 0.8f); // Минимальный размер камня
    public Vector3 maxScale = new Vector3(1.5f, 1.5f, 1.5f); // Максимальный размер камня

    public bool generateOnStart = true; // Деформация при старте игры

    private Mesh originalMesh;
    private string savedMeshPath;

    private void Start()
    {
        savedMeshPath = "Assets/Resources/savedRockMesh.asset";  // Путь к сохранённому мешу
        originalMesh = GetComponent<MeshFilter>().mesh;

        if (generateOnStart)
        {
            GenerateRock();
            InvokeRepeating(nameof(GenerateRock), 1, 1);
        }

        // Загружаем сохранённый меш, если он есть
        if (System.IO.File.Exists(savedMeshPath))
        {
            LoadMesh();
        }
    }

    private void Update()
    {
        // Сохранение меша при нажатии на P
        if (Input.GetKeyDown(KeyCode.P))
        {
            SaveMesh();
        }

        // Возвращение к исходному мешу при нажатии на L
        if (Input.GetKeyDown(KeyCode.L))
        {
            RestoreOriginalMesh();
        }
    }

    public void GenerateRock()
    {
        // Получение MeshFilter
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null || meshFilter.mesh == null)
        {
            Debug.LogError("У объекта отсутствует MeshFilter или Mesh.");
            return;
        }

        // Копируем оригинальный меш
        Mesh deformedMesh = Instantiate(originalMesh);

        Vector3[] vertices = deformedMesh.vertices;

        // Случайная деформация вершин
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 vertex = vertices[i];

            // Генерация случайного смещения
            for (int j = 0; j < noiseIterations; j++)
            {
                Vector3 noise = new Vector3(
                    Mathf.PerlinNoise(vertex.x * noiseScale + j, vertex.y * noiseScale + j) - 0.5f,
                    Mathf.PerlinNoise(vertex.y * noiseScale + j, vertex.z * noiseScale + j) - 0.5f,
                    Mathf.PerlinNoise(vertex.z * noiseScale + j, vertex.x * noiseScale + j) - 0.5f
                );

                noise *= deformationIntensity / noiseIterations;
                vertex += noise;
            }

            vertices[i] = vertex;
        }

        // Обновление меша
        deformedMesh.vertices = vertices;
        deformedMesh.RecalculateNormals();
        deformedMesh.RecalculateBounds();

        meshFilter.mesh = deformedMesh;

        // Обновление MeshCollider (если есть)
        MeshCollider meshCollider = GetComponent<MeshCollider>();
        if (meshCollider != null)
        {
            meshCollider.sharedMesh = null;
            meshCollider.sharedMesh = deformedMesh;
        }

        // Случайный масштаб камня
        transform.localScale = new Vector3(
            Random.Range(minScale.x, maxScale.x),
            Random.Range(minScale.y, maxScale.y),
            Random.Range(minScale.z, maxScale.z)
        );
    }

    // Сохранение текущего меша
    private void SaveMesh()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null || meshFilter.mesh == null)
        {
            Debug.LogError("У объекта отсутствует MeshFilter или Mesh.");
            return;
        }

        Mesh meshToSave = meshFilter.mesh;
        string filePath = savedMeshPath;

        // Использование AssetDatabase для сохранения меша
        AssetDatabase.CreateAsset(meshToSave, filePath);
        AssetDatabase.SaveAssets();
        Debug.Log("Меш сохранён в файл: " + filePath);
    }

    // Загрузка сохранённого меша
    private void LoadMesh()
    {
        Mesh savedMesh = AssetDatabase.LoadAssetAtPath<Mesh>(savedMeshPath);
        if (savedMesh != null)
        {
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            meshFilter.mesh = savedMesh;
            Debug.Log("Загружен сохранённый меш.");
        }
        else
        {
            Debug.LogWarning("Сохранённый меш не найден.");
        }
    }

    // Восстановление исходного меша
    private void RestoreOriginalMesh()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter != null)
        {
            meshFilter.mesh = originalMesh;
            Debug.Log("Меш восстановлен до исходного состояния.");
        }
    }
}
