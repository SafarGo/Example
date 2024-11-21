using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    public bool allowMoveX = true; // Разрешено ли перемещение по X
    public bool allowMoveY = false; // Разрешено ли перемещение по Y
    public bool allowMoveZ = true; // Разрешено ли перемещение по Z
    public GridManager gridManager;

    private Camera mainCamera;
    private bool isDragging = false;
    private Vector3 offset;
    private Vector3 targetPosition; // Позиция, к которой объект движется
    public float moveSpeed = 10f; // Скорость перемещения
    public float detectionRadius = 2f; // Радиус для поиска других объектов
    public LayerMask collisionLayer; // Слой, на котором будут другие объекты

    private Renderer[] renderers; // Массив для изменения цветов всех Renderer'ов
    private Color originalColor; // Оригинальный цвет объекта
    private bool isRed = false; // Флаг, чтобы отслеживать, стал ли объект красным
    private Vector3 positionWhenNotRed; // Позиция объекта, когда он не был красным

    void Start()
    {
        mainCamera = Camera.main;
        targetPosition = transform.position; // Начальная цель — текущая позиция

        // Сохраняем исходную позицию объекта
        positionWhenNotRed = transform.position;

        // Собираем все компоненты Renderer для текущего объекта и его дочерних объектов
        renderers = GetComponentsInChildren<Renderer>();

        if (renderers.Length > 0)
        {
            // Сохраняем оригинальный цвет первого найденного Renderer
            originalColor = renderers[0].material.color;
        }
        else
        {
            Debug.LogWarning("No Renderer components found on " + gameObject.name + " or its children.");
        }
    }

    void Update()
    {
        // Постепенно перемещаем объект к целевой позиции
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Проверка на другие объекты поблизости
        CheckForNearbyObjects();

        if (Input.GetKeyDown(KeyCode.R) && isDragging)
        {
            Debug.Log("SUUUUUUUUUUUUUUUUUUUUUUU");
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

            // Устанавливаем новую целевую позицию
            targetPosition = newPosition;
        }
    }

    void OnMouseUp()
    {
        isDragging = false;

        // Если объект стал красным, запоминаем его позицию, где он был не красным
        if (isRed)
        {
            targetPosition = positionWhenNotRed; // Начинаем движение в исходную позицию
            foreach (var renderer in renderers)
            {
                renderer.material.color = originalColor;
            }
        }
    }

    void CheckForNearbyObjects()
    {
        // Проверка на столкновение с другими объектами в радиусе
        Collider[] nearbyObjects = Physics.OverlapSphere(transform.position, detectionRadius, collisionLayer);

        foreach (var collider in nearbyObjects)
        {
            // Игнорируем столкновение с самим собой
            if (collider.gameObject == gameObject)
                continue;

            // Если есть объекты рядом, меняем цвет всех Renderer'ов на красный
            if (!isRed) // Если объект еще не стал красным, то меняем его цвет
            {
                foreach (var renderer in renderers)
                {
                    renderer.material.color = Color.red;
                }
                isRed = true; // Устанавливаем флаг, что объект стал красным
                positionWhenNotRed = transform.position; // Запоминаем текущую позицию, когда объект стал красным
            }
        }

        // Если объектов нет рядом и объект стал красным, восстанавливаем исходный цвет
        if (nearbyObjects.Length == 0 && isRed)
        {
            RestoreOriginalColor();
            isRed = false; // Убираем флаг, что объект стал красным
        }
    }

    void RestoreOriginalColor()
    {
        // Восстанавливаем исходный цвет всех Renderer'ов
        foreach (var renderer in renderers)
        {
            renderer.material.color = originalColor;
        }
    }
}
