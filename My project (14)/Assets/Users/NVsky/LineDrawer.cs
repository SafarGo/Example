using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    public float raycastLength = 10f; // Длина рейкаста
    public Transform startPoint; // Стартовая точка (будет выбрана с помощью рейкаста)
    public Transform targetPoint; // Целевая точка (будет выбрана с помощью рейкаста)
    public GameObject itemPrefab; // Префаб для создания пути
    public float itemSpacing = 1f; // Расстояние между объектами на пути

    private Camera mainCamera;
    private bool isStartPointSelected = false;
    private bool isTargetPointSelected = false;
    private bool pathSpawned = false; // Флаг, чтобы спавнить путь только один раз

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Реализуем выбор точек с помощью рейкаста и клавиши "T"
        if (Input.GetKeyDown(KeyCode.T))
        {
            TrySelectPoint();
        }

        // Если обе точки выбраны и путь еще не был создан, прокладываем путь
        if (isStartPointSelected && isTargetPointSelected && !pathSpawned)
        {
            // Спавним объекты вдоль пути
            SpawnItemsBetweenTransforms(startPoint, targetPoint);
            pathSpawned = true; // Устанавливаем флаг, чтобы путь больше не создавался
        }
    }

    void TrySelectPoint()
    {
        // Рейкаст из камеры в направлении взгляда
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastLength))
        {
            // Проверяем, что мы попали в объект, с которым можем работать
            if (!isStartPointSelected)
            {
                // Выбираем стартовую точку
                startPoint = hit.transform;
                isStartPointSelected = true;
                Debug.Log("Start point selected");
            }
            else if (!isTargetPointSelected)
            {
                // Выбираем целевую точку
                targetPoint = hit.transform;
                isTargetPointSelected = true;
                Debug.Log("Target point selected");
            }
            else
            {
                // Если уже выбраны обе точки, сбрасываем их, если рейкаст не попал в объект
                startPoint = null;
                targetPoint = null;
                isStartPointSelected = false;
                isTargetPointSelected = false;
                pathSpawned = false; // Сбрасываем флаг, чтобы путь мог быть снова проложен
                Debug.Log("Points reset");
            }
        }
        else
        {
            // Если рейкаст не попал в объект, сбрасываем точки
            startPoint = null;
            targetPoint = null;
            isStartPointSelected = false;
            isTargetPointSelected = false;
            pathSpawned = false; // Сбрасываем флаг, чтобы путь мог быть снова проложен
            Debug.Log("Points reset");
        }
    }

    void SpawnItemsBetweenTransforms(Transform start, Transform target)
    {
        // Если точки не выбраны, не делаем ничего
        if (start == null || target == null) return;

        // Игнорируем разницу по оси Y, обнуляем её для обеих точек
        Vector3 startPosition = new Vector3(start.position.x, 0, start.position.z);
        Vector3 targetPosition = new Vector3(target.position.x, 0, target.position.z);

        // Вычисляем направление от стартовой точки к целевой
        Vector3 direction = (targetPosition - startPosition).normalized;

        // Вычисляем расстояние между точками
        float distance = Vector3.Distance(startPosition, targetPosition);

        // Определяем количество объектов для спавна вдоль пути
        int itemCount = Mathf.FloorToInt(distance / itemSpacing);

        for (int i = 0; i < itemCount; i++)
        {
            // Вычисляем позицию каждого предмета вдоль пути
            Vector3 spawnPosition = startPosition + direction * i * itemSpacing;

            // Создаем объект
            GameObject item = Instantiate(itemPrefab, spawnPosition, Quaternion.identity);

            // Поворачиваем объект в сторону стартовой точки
            Vector3 lookDirection = (startPosition - spawnPosition).normalized;

            // Поворот объекта по оси Y в сторону целевой точки
            item.transform.rotation = Quaternion.LookRotation(lookDirection);
        }

        Debug.Log("Path spawned");
    }
}
