using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshObstacle))]
public class ObjectPlacer : MonoBehaviour
{
    [Header("Settings")]
    public LayerMask raycastLayerMask;
    public string[] collisionTags; // Теги для проверки столкновений
    public float checkRadius = 0.5f; // Радиус проверки на свободное место
    public int maxSearchIterations = 10; // Максимальное количество попыток поиска
    Renderer objectRenderer;

    [Header("Camera Settings")]
    public Camera targetCamera; // Камера для рейкаста
    [SerializeField] int maxDistanceFromCamera = 50;
    [SerializeField] int minDistanceFromCamera = 3;

    [Header("Grid Settings")]
    public float gridSize = 1f; // Размер сетки
    public float minGridSize = 0.1f;
    public float maxGridSize = 5f;
    public float gridStep = 0.1f;

    [Header("Smoothing Settings")]
    public float smoothingSpeed = 10f; // Скорость плавного перемещения

    private Color originalColor;

    private bool canPlace = true;
    private Vector3 targetPosition;
    private Vector3 firstPosition;
    private bool isDragging = false;

    private Plane movementPlane; // Плоскость для перемещения объекта

    private static float globalGridSize = 1f; // Общий размер сетки//было флот
    public string ObjectType;
    public int ObjectID;

    [Header("Terrain Settings")]
    public string terrainTag = "Terrain"; // Тег для террейна
    public float maxSlopeAngle = 30f; // Максимально допустимый угол наклона
    Vector3 lastCtrlPosition;
    Vector3 lastPosition;

    private void Awake()
    {
        StaticHolder.AddObstacle(this);
    }
    void Start()
    {
        globalGridSize = 0.2f;
        objectRenderer = GetComponent<Renderer>();
        if (targetCamera == null)
        {
            targetCamera = Camera.main; // Используем главную камеру, если другая не указана
        }

        if (objectRenderer != null)
        {
            originalColor = objectRenderer.material.color;
        }

        targetPosition = transform.position;
        firstPosition = transform.position;

        // Плоскость для перемещения на уровне объекта
        movementPlane = new Plane(Vector3.up, transform.position);
    }

    void Update()
    {
        if (StaticHolder.isCanBuild)
        {
            if (Input.GetMouseButtonDown(0))
            {
                CheckObjectHit();
            }

            if (Input.GetMouseButton(0) && isDragging)
            {
                DragObject();
            }
        }

            if (Input.GetMouseButtonUp(0) && isDragging)
            {
                StopDragging();
            }

            AdjustGlobalGridSize();

            // Плавное перемещение объекта к целевой позиции
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothingSpeed);

    }

    void CheckObjectHit()
    {
        Ray ray = targetCamera.ScreenPointToRay(Input.mousePosition);

        // Находим все объекты, с которыми пересекается рейкаст
        RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, raycastLayerMask);

        if (hits.Length > 0)
        {
            RaycastHit nearestHit = hits[0];
            float nearestDistance = Vector3.Distance(targetCamera.transform.position, hits[0].point);

            // Проходимся по всем пересечениям и находим ближайший объект
            foreach (RaycastHit hit in hits)
            {
                float distance = Vector3.Distance(targetCamera.transform.position, hit.point);
                if (distance < nearestDistance)
                {
                    nearestHit = hit;
                    nearestDistance = distance;
                }
            }

            // Убедимся, что ближайший объект — это текущий объект
            if (nearestHit.collider != null && nearestHit.collider.gameObject == gameObject)
            {
                StartDragging();
            }
        }
    }

    void StartDragging()
    {
        isDragging = true;
        targetPosition = transform.position; // Устанавливаем текущую позицию объекта как начальную
    }

    void DragObject()
    {

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            StopDragging();
            return;
        }
        if (canPlace)
        {
            lastPosition = gameObject.transform.position;
        }

        Ray ray = targetCamera.ScreenPointToRay(Input.mousePosition);

        // Используем движение по плоскости на высоте объекта
        if (movementPlane.Raycast(ray, out float enter))
        {
            Vector3 hitPosition = ray.GetPoint(enter);

            // Привязка к глобальной сетке при зажатом Ctrl
            if (Input.GetKey(KeyCode.LeftControl))
            {
                hitPosition.x = Mathf.Round(hitPosition.x / globalGridSize) * globalGridSize;
                hitPosition.z = Mathf.Round(hitPosition.z / globalGridSize) * globalGridSize;
                hitPosition.y = Mathf.Round(transform.position.y / globalGridSize) * globalGridSize; // Фиксируем высоту
                lastCtrlPosition = hitPosition;
            }

            // Фиксируем высоту объекта
            hitPosition.y = transform.position.y;

            // Проверяем расстояние от камеры
            float distance = Vector3.Distance(hitPosition, targetCamera.transform.position);

            if (distance >= minDistanceFromCamera && distance <= maxDistanceFromCamera)
            {
                targetPosition = hitPosition;
            }
            else
            {
                // Если объект слишком близко или слишком далеко от камеры, ограничиваем его позицию
                if (distance < minDistanceFromCamera)
                {
                    // Ограничиваем объект минимальным расстоянием
                    Vector3 direction = (hitPosition - targetCamera.transform.position).normalized;
                    targetPosition = targetCamera.transform.position + direction * minDistanceFromCamera;
                }
                else
                {
                    // Ограничиваем объект максимальным расстоянием
                    Vector3 direction = (hitPosition - targetCamera.transform.position).normalized;
                    targetPosition = targetCamera.transform.position + direction * maxDistanceFromCamera;
                }

                // Сохраняем оригинальную высоту объекта
                targetPosition.y = transform.position.y;
            }
        }

        // Поворот объекта при нажатии клавиши R
        if (Input.GetKeyDown(KeyCode.R))
        {
            gameObject.transform.Rotate(0, 90, 0);
        }
    }

    void StopDragging()
    {
        isDragging = false;

        if (!canPlace)
        {
            Vector3 freePosition = FindNearestFreePosition(gameObject.transform.position);
            //Vector3 freePosition = firstPosition;
            targetPosition = freePosition;

            Debug.Log($"Нельзя поставить объект на это место! Перемещено в ближайшую свободную точку: {freePosition}");
        }
        else
        {
            firstPosition = transform.position;
        }
        SetObjectColor(originalColor);
    }

    void AdjustGlobalGridSize()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f && Input.GetKey(KeyCode.LeftControl))
        {
            globalGridSize = Mathf.Clamp(globalGridSize + scroll * gridStep, minGridSize, maxGridSize);
            Debug.Log($"Размер глобальной сетки: {globalGridSize}");
        }
        Debug.Log(globalGridSize);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsCollisionWithInvalidTag(other) && isDragging)
        {
            canPlace = false;
            SetObjectColor(Color.red);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (IsCollisionWithInvalidTag(other) && isDragging)
        {
            canPlace = false;
            SetObjectColor(Color.red);
        }
        else
        {
            if (IsCollisionWithInvalidTag(other) && isDragging)
            {
                canPlace = true;
                SetObjectColor(Color.green);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsCollisionWithInvalidTag(other) && isDragging)
        {
            canPlace = true;
            SetObjectColor(Color.green);
        }
    }

    private bool IsCollisionWithInvalidTag(Collider other)
    {
        if (other.transform.IsChildOf(transform)) return false;

        foreach (string tag in collisionTags)
        {
            if (other.gameObject.tag == "Terrain")
            {
                canPlace = false;
                isDragging = false;
                StopDragging();
            }
            if (other.CompareTag(tag)) return true;
        }

        return false;
    }

    private void SetObjectColor(Color color)
    {
        // Меняем цвет для самого объекта
        if (objectRenderer != null)
        {
            objectRenderer.material.color = color;
        }

        // Меняем цвет для всех дочерних объектов
        foreach (Transform child in transform)
        {
            Renderer childRenderer = child.GetComponent<Renderer>();
            if (childRenderer != null)
            {
                childRenderer.material.color = color;
            }
        }
    }

    private void OnDisable()
    {
        SetObjectColor(originalColor); // Сброс цвета при отключении объекта
    }

    private Vector3 FindNearestFreePosition(Vector3 startPosition)
    {
        startPosition = lastPosition;
        return startPosition;
    }
}
