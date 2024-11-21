using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    public bool allowMoveX = true; // Разрешено ли перемещение по X
    public bool allowMoveY = false; // Разрешено ли перемещение по Y
    public bool allowMoveZ = true; // Разрешено ли перемещение по Z
    public GridManager gridManager; // Ссылка на GridManager

    private Camera mainCamera;
    private bool isDragging = false;
    private Vector3 offset;
    private Vector3 targetPosition; // Позиция, к которой движется объект
    public float moveSpeed = 10f; // Скорость перемещения

    public float detectionRadius = 2f; // Радиус для проверки столкновений
    public LayerMask collisionLayer; // Слой для проверки столкновений

    private Renderer[] renderers; // Все рендереры текущего объекта и его дочерних объектов
    private Material[] originalMaterials; // Исходные материалы объекта
    private Vector3 lastValidPosition; // Последняя позиция без столкновений
    private bool isRed = false; // Флаг, указывающий, стал ли объект красным

    void Start()
    {
        mainCamera = Camera.main;
        targetPosition = transform.position; // Начальная цель — текущая позиция

        // Получаем все рендереры текущего объекта и его дочерних объектов
        renderers = GetComponentsInChildren<Renderer>();
        originalMaterials = new Material[renderers.Length];

        // Сохраняем оригинальные материалы
        for (int i = 0; i < renderers.Length; i++)
        {
            originalMaterials[i] = renderers[i].material;
        }

        lastValidPosition = transform.position; // Запоминаем начальную позицию как валидную
    }

    void Update()
    {
        // Постепенно перемещаем объект к целевой позиции
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (isDragging)
        {
            // Проверяем столкновения с другими объектами
            CheckCollisions();
        }

        if (Input.GetKeyDown(KeyCode.R) && isDragging)
        {
            Debug.Log("Rotating Object");
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

            // Ограничиваем перемещение только разрешенными осями
            newPosition = new Vector3(
                allowMoveX ? newPosition.x : transform.position.x,
                allowMoveY ? newPosition.y : transform.position.y,
                allowMoveZ ? newPosition.z : transform.position.z
            );

            // Привязываем позицию к ближайшим координатам сетки из GridManager
            if (gridManager != null)
            {
                newPosition = gridManager.GetClosestGridPoint(newPosition);
            }
            else
            {
                Debug.LogWarning("GridManager не назначен!");
            }

            // Устанавливаем новую целевую позицию
            targetPosition = newPosition;
        }
    }

    void OnMouseUp()
    {
        isDragging = false;

        if (isRed)
        {
            // Если объект красный, возвращаем его в последнюю допустимую позицию
            targetPosition = lastValidPosition;
            RestoreOriginalMaterials(); // Восстанавливаем оригинальные материалы
            isRed = false;
        }
        else
        {
            // Если объект не красный, текущая позиция становится валидной
            lastValidPosition = transform.position;
        }
    }

    void CheckCollisions()
    {
        Collider[] nearbyObjects = Physics.OverlapSphere(transform.position, detectionRadius, collisionLayer);

        bool hasCollision = false; // Флаг для проверки столкновений

        foreach (Collider collider in nearbyObjects)
        {
            // Игнорируем столкновение с самим собой
            if (collider.gameObject != gameObject)
            {
                hasCollision = true;
                break;
            }
        }

        if (hasCollision)
        {
            // Если есть столкновения, меняем материалы всех дочерних объектов на красный
            if (!isRed)
            {
                SetMaterialToColor(Color.red);
                isRed = true;
            }
        }
        else
        {
            // Если столкновений нет, возвращаем исходные материалы
            if (isRed)
            {
                RestoreOriginalMaterials();
                isRed = false;
            }
        }
    }

    void SetMaterialToColor(Color color)
    {
        // Изменяем цвет всех материалов рендеров объекта и его дочерних объектов
        foreach (Renderer renderer in renderers)
        {
            Material newMaterial = new Material(renderer.material);
            newMaterial.color = color;
            renderer.material = newMaterial;
        }
    }

    void RestoreOriginalMaterials()
    {
        // Восстанавливаем оригинальные материалы всех рендеров объекта и его дочерних объектов
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material = originalMaterials[i];
        }
    }
}
