using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneOptimizer : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private bool useStaticBatching = true;
    [SerializeField] private bool useLODSupport = true;
    [SerializeField] private float cullingDistance = 50f;

    [Header("Tag Exceptions")]
    [SerializeField] private string[] excludedTags; // Теги объектов, которые нельзя отключать

    [Header("Pooling Settings")]
    [SerializeField] private int poolSize = 100;

    private ObjectPool objectPool;

    void Start()
    {
        // Статическое объединение
        if (useStaticBatching)
            StaticBatchingUtility.Combine(gameObject);

        // Установка дистанции отрисовки для всех объектов
        SetCullingDistance(cullingDistance);

        // Инициализация пула объектов
        objectPool = new ObjectPool(poolSize);
    }

    void SetCullingDistance(float distance)
    {
        foreach (var renderer in FindObjectsOfType<Renderer>())
        {
            if (renderer.gameObject.isStatic)
            {
                var lodGroup = renderer.GetComponent<LODGroup>();
                if (useLODSupport && lodGroup != null)
                    lodGroup.RecalculateBounds();
                else
                    renderer.enabled = Vector3.Distance(Camera.main.transform.position, renderer.transform.position) <= distance;
            }
        }
    }

    void Update()
    {
        // Динамическое отключение объектов за камерой, кроме объектов с исключёнными тегами
        foreach (var renderer in FindObjectsOfType<Renderer>())
        {
            if (!renderer.isVisible && !IsExcluded(renderer.gameObject) && !renderer.gameObject.isStatic)
            {
                renderer.enabled = false; // Отключаем объект
            }
            else if (!renderer.enabled && IsExcluded(renderer.gameObject) || renderer.isVisible)
            {
                renderer.enabled = true; // Включаем объект, если он виден или исключён по тегу
            }
        }
    }

    bool IsExcluded(GameObject obj)
    {
        foreach (string tag in excludedTags)
        {
            if (obj.CompareTag(tag))
                return true;
        }
        return false;
    }
}

public class ObjectPool
{
    private GameObject[] poolObjects;
    private int currentIndex;

    public ObjectPool(int size)
    {
        poolObjects = new GameObject[size];
        currentIndex = 0;
    }

    public GameObject GetObject(GameObject prefab)
    {
        if (poolObjects[currentIndex] == null)
        {
            poolObjects[currentIndex] = Object.Instantiate(prefab);
        }

        var obj = poolObjects[currentIndex];
        currentIndex = (currentIndex + 1) % poolObjects.Length;
        obj.SetActive(true);
        return obj;
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
    }
}
