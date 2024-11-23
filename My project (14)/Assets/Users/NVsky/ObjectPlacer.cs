using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    public bool allowMoveX = true;
    public bool allowMoveY = false;
    public bool allowMoveZ = true;
    public GridManager gridManager;

    private Camera mainCamera;
    private bool isDragging = false;
    private Vector3 offset;
    private Vector3 targetPosition;
    public float moveSpeed = 10f;

    public float detectionRadius = 2f;
    public LayerMask collisionLayer;

    [Header("Имя сетки")]
    [SerializeField] string gridName;

    private Renderer[] renderers;
    private Material[] originalMaterials;
    private Vector3 lastValidPosition;
    private bool isRed = false;

    void Start()
    {
        mainCamera = Camera.main;
        targetPosition = transform.position;

        renderers = GetComponentsInChildren<Renderer>();
        originalMaterials = new Material[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            originalMaterials[i] = renderers[i].material;
        }

        lastValidPosition = transform.position;
        if (gridManager == null)
        {
            gridManager = GameObject.Find(gridName).GetComponent<GridManager>();
        }

        // Проверка начальной позиции
        CheckInitialPosition();
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (isDragging)
        {
            CheckCollisions();
        }

        // Вращение объекта
        if (Input.GetKeyDown(KeyCode.R) && isDragging)
        {
            transform.Rotate(0, 90, 0);
        }
    }

    void OnMouseDown()
    {
        isDragging = true;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            offset = transform.position - hit.point;
        }
    }

    void OnMouseDrag()
    {
        if (!isDragging) return;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 newPosition = hit.point + offset;

            newPosition = new Vector3(
                allowMoveX ? newPosition.x : transform.position.x,
                allowMoveY ? newPosition.y : transform.position.y,
                allowMoveZ ? newPosition.z : transform.position.z
            );

            if (gridManager != null)
            {
                newPosition = gridManager.GetClosestGridPoint(newPosition);
            }
            else
            {
                Debug.LogWarning("GridManager не назначен!");
            }

            targetPosition = newPosition;
        }
    }

    void OnMouseUp()
    {
        isDragging = false;

        // Восстановление оригинальных материалов или возвращение в последнюю валидную позицию
        if (isRed)
        {
            targetPosition = lastValidPosition;
            RestoreOriginalMaterials();
            isRed = false;
        }
        else
        {
            lastValidPosition = transform.position;
        }
    }

    void CheckCollisions()
    {
        // Проверка коллизий с другими объектами
        Collider[] nearbyObjects = Physics.OverlapSphere(transform.position, detectionRadius, collisionLayer);

        bool hasCollision = false;

        foreach (Collider collider in nearbyObjects)
        {
            if (collider.gameObject != gameObject)
            {
                hasCollision = true;
                break;
            }
        }

        // Если есть коллизия
        if (hasCollision)
        {
            if (!isRed)
            {
                SetMaterialToColor(Color.red);
                isRed = true;
            }
        }
        else
        {
            if (isRed)
            {
                RestoreOriginalMaterials();
                isRed = false;
            }
        }
    }

    void CheckInitialPosition()
    {
        // Проверка на старте
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, collisionLayer);

        // Если в начальной позиции есть коллизия с другим объектом, то начинаем поиск
        if (colliders.Length > 0)
        {
            Debug.Log("Объект появился внутри другого объекта. Ищем ближайшую свободную точку...");
            FindNearestFreePosition();
        }
        else
        {
            Debug.Log("На старте объект не сталкивается с другими объектами, остаемся на месте.");
        }
    }

    void FindNearestFreePosition()
    {
        Vector3 closestPoint = transform.position;
        bool foundFreePoint = false;

        // Поиск ближайшей свободной позиции с учётом сетки и коллизий
        float maxSearchRadius = detectionRadius * 5f; // Максимальный радиус поиска
        float searchRadiusStep = detectionRadius; // Шаг увеличения радиуса поиска

        for (float radius = detectionRadius; radius <= maxSearchRadius; radius += searchRadiusStep)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius, collisionLayer);

            // Пробуем переместиться в новые возможные позиции
            foreach (Collider collider in colliders)
            {
                if (collider.gameObject == gameObject)
                    continue; // Игнорируем сам объект

                Vector3 potentialPoint = transform.position + Random.insideUnitSphere * radius;

                // Привязка к сетке, но высота остается постоянной
                if (gridManager != null)
                {
                    potentialPoint = gridManager.GetClosestGridPoint(potentialPoint);
                }

                potentialPoint.y = transform.position.y; // Высота не меняется

                // Проверка на коллизии с другими объектами
                Collider[] nearbyColliders = Physics.OverlapSphere(potentialPoint, detectionRadius, collisionLayer);
                if (nearbyColliders.Length == 0)
                {
                    closestPoint = potentialPoint;
                    foundFreePoint = true;
                    break;
                }
            }

            if (foundFreePoint) break;
        }

        // Перемещаем объект на найденную точку
        if (foundFreePoint)
        {
            targetPosition = new Vector3(closestPoint.x, transform.position.y, closestPoint.z);
            lastValidPosition = targetPosition;
            RestoreOriginalMaterials();
            isRed = false;
        }
        else
        {
            Debug.LogWarning("Не удалось найти свободное место!");
        }
    }

    void SetMaterialToColor(Color color)
    {
        foreach (Renderer renderer in renderers)
        {
            Material newMaterial = new Material(renderer.material);
            newMaterial.color = color;
            renderer.material = newMaterial;
        }
    }

    void RestoreOriginalMaterials()
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material = originalMaterials[i];
        }
    }
}