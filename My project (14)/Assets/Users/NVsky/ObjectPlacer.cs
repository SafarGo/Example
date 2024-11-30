using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField] GameObject BuildingMenu;
    public bool allowMoveX = true;
    public bool allowMoveY = false;
    public bool allowMoveZ = true;

    private Camera mainCamera;
    private bool isDragging = false;
    private Vector3 offset;
    private Vector3 targetPos;
    public float moveSpeed = 10f;

    public LayerMask collisionLayer; // Слой для коллизий
    private Vector3 lastValidPosition;
    private Quaternion currentRotation = Quaternion.identity;
    [Header("ID для спавна")]
    public int ObjectSpawnerID;

    public int ObjectID;

    private void Start()
    {
        mainCamera = Camera.main;
        targetPos = transform.position;

        lastValidPosition = transform.position;

        // Проверка на коллизию при старте, если объект заспавнился в другом объекте
        ////////обязательно, но переделать/////FindNearestEmptyPosition();
    }

    private void Update()
    {
        if (!isDragging) return;

        HandleRotation(); // Обработка вращения
        if (isDragging)
        {
            MoveObjectWithCollisions();
        }
    }

    private void OnMouseDown()
    {
        isDragging = true;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            offset = transform.position - hit.point;
        }
    }

    private void OnMouseDrag()
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

            if (!IsColliding(newPosition, currentRotation))
            {
                targetPos = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * moveSpeed);
                lastValidPosition = targetPos;
            }
            else
            {
                // Если коллизия, оставляем объект в текущем положении
                transform.position = lastValidPosition;
            }
        }
    }

    private void OnMouseUp()
    {
        StaticHolder.AllSpawnedObjectsTranforms[ObjectID] = gameObject.transform.position;
        StaticHolder.AllSpawnedObjectsRotations[ObjectID] = gameObject.transform.rotation;
        JsonSaver._instance.Save();
        Debug.Log("сохранили при передвижении");
        isDragging = false;
    }

    private void HandleRotation()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            currentRotation *= Quaternion.Euler(0, 90, 0); // Поворот на 90 градусов по оси Y
            transform.rotation = currentRotation;

            if (IsColliding(transform.position, currentRotation))
            {
                // Если столкновение, вернуться к последней корректной позиции
                transform.position = lastValidPosition;
            }
            else
            {
                lastValidPosition = transform.position; // Обновляем последнюю корректную позицию
            }
        }
    }

    private bool IsColliding(Vector3 newPosition, Quaternion rotation)
    {
        Collider[] colliders = Physics.OverlapBox(newPosition, GetComponent<Collider>().bounds.extents, rotation, collisionLayer);

        foreach (Collider col in colliders)
        {
            // Игнорировать столкновения с собой и дочерними объектами
            if (col.gameObject != gameObject && !col.transform.IsChildOf(transform))
            {
                return true;
            }
        }
        return false;
    }

    private void MoveObjectWithCollisions()
    {
        Vector3 newPosition = targetPos;

        // Проверка на столкновение с другими объектами
        if (IsColliding(newPosition, currentRotation))
        {
            transform.position = lastValidPosition; // Вернуть к последней корректной позиции, если столкновение
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, newPosition, moveSpeed * Time.deltaTime);
        }
    }

    // Метод для поиска ближайшего свободного места
    private void FindNearestEmptyPosition()
    {
        Vector3 closestPosition = transform.position;
        float searchRadius = 5f;
        float stepSize = 1f;
        bool foundFreeSpot = false;

        for (float x = transform.position.x - searchRadius; x <= transform.position.x + searchRadius; x += stepSize)
        {
            for (float z = transform.position.z - searchRadius; z <= transform.position.z + searchRadius; z += stepSize)
            {
                Vector3 newPos = new Vector3(x, transform.position.y, z);

                if (!IsColliding(newPos, currentRotation)) // Учитываем текущий поворот
                {
                    closestPosition = newPos;
                    foundFreeSpot = true;
                    break;
                }
            }
            if (foundFreeSpot) break;
        }

        transform.position = closestPosition;
    }

    // Используем OnCollisionStay или OnTriggerStay для более точной обработки столкновений
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("obstacle_tag"))
        {
            // Обработка столкновения с объектами с тегом obstacle_tag
            Debug.Log("Столкновение с объектом: " + collision.gameObject.name);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("obstacle_tag"))
        {
            // Обработка столкновения с объектами с тегом obstacle_tag
            Debug.Log("Триггер с объектом: " + other.gameObject.name);
        }
    }
}
