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
    [HideInInspector] public bool isSpawning = false;
    public int ObjectID;
    private Material[] originalMaterials;

    private Renderer[] renderers;
    private MaterialPropertyBlock propertyBlock;
    private Dictionary<Renderer, Color[]> originalColors;

    // Параметры куба для визуализации коллизий
    [Header("Настройки куба коллизий")]
    [SerializeField] private Vector3 collisionCubeSize = Vector3.one; // Размер куба
    [SerializeField] private Vector3 collisionCubeOffset = Vector3.zero; // Смещение куба
    [SerializeField] private Vector3 collisionCubeRotation = Vector3.zero; // Поворот куба

    private void Start()
    {
        mainCamera = Camera.main;
        targetPos = transform.position;

        lastValidPosition = transform.position;

        renderers = GetComponentsInChildren<Renderer>();
        propertyBlock = new MaterialPropertyBlock();

        originalMaterials = new Material[renderers.Length];
        originalColors = new Dictionary<Renderer, Color[]>();

        foreach (Renderer renderer in renderers)
        {
            Material[] materials = renderer.materials;
            Color[] colors = new Color[materials.Length];
            for (int i = 0; i < materials.Length; i++)
            {
                colors[i] = materials[i].color;
            }
            originalColors[renderer] = colors;
        }

        if (IsColliding(gameObject.transform.position, currentRotation))
        {
            FindNearestEmptyPosition();
        }
    }

    private void Update()
    {
        if (!isDragging) return;

        HandleRotation();
        MoveObjectWithCollisions();
    }

    private void OnMouseDown()
    {
        isDragging = true;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            offset = transform.position - hit.point;
        }

        SetMaterialToColor(Color.green);
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
                transform.position = lastValidPosition;
            }
        }
    }

    private void OnMouseUp()
    {
        isDragging = false;

        RestoreOriginalMaterials();

        StaticHolder.AllSpawnedObjectsTranforms[ObjectID] = gameObject.transform.position;
        StaticHolder.AllSpawnedObjectsRotations[ObjectID] = gameObject.transform.rotation;
        JsonSaver._instance.Save();
        Debug.Log("сохранили при передвижении");
    }

    void SetMaterialToColor(Color color)
    {
        foreach (Renderer renderer in renderers)
        {
            Material[] materials = renderer.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                Material newMaterial = new Material(materials[i]);
                newMaterial.color = color;
                materials[i] = newMaterial;
            }
            renderer.materials = materials;
        }
    }

    void RestoreOriginalMaterials()
    {
        foreach (Renderer renderer in renderers)
        {
            Material[] materials = renderer.materials;
            Color[] originalColorArray = originalColors[renderer];

            for (int i = 0; i < materials.Length; i++)
            {
                materials[i].color = originalColorArray[i];
            }
            renderer.materials = materials;
        }
    }

    private void HandleRotation()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            currentRotation *= Quaternion.Euler(0, 90, 0);
            transform.rotation = currentRotation;

            if (IsColliding(transform.position, currentRotation))
            {
                transform.position = lastValidPosition;
            }
            else
            {
                lastValidPosition = transform.position;
            }
        }

        if(Input.GetKeyDown(KeyCode.Y))
        {
            StaticHolder.AllSpawnedObjectsID.RemoveAt(ObjectID);
            StaticHolder.AllSpawnedObjectsTranforms.RemoveAt(ObjectID);
            StaticHolder.AllSpawnedObjectsRotations.RemoveAt(ObjectID);
            JsonSaver._instance.Save();
            Destroy(gameObject);
        }
    }

    private bool IsColliding(Vector3 newPosition, Quaternion rotation)
    {
        Collider[] colliders = Physics.OverlapBox(newPosition, GetComponent<Collider>().bounds.extents, rotation, collisionLayer);

        foreach (Collider col in colliders)
        {
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

        if (IsColliding(newPosition, currentRotation))
        {
            transform.position = lastValidPosition;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, newPosition, moveSpeed * Time.deltaTime);
        }
    }

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

                if (!IsColliding(newPos, currentRotation))
                {
                    closestPosition = newPos;
                    foundFreeSpot = true;
                    break;
                }
            }
            if (foundFreeSpot) break;
        }

        StartCoroutine(MoveToPosition(closestPosition, 1f));
    }

    private IEnumerator MoveToPosition(Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.position = targetPosition;
    }

    // Добавляем метод для отрисовки кастомного куба коллизий
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        // Вычисляем матрицу для отрисовки с учётом настроек
        Matrix4x4 matrix = Matrix4x4.TRS(
            transform.position + collisionCubeOffset,
            transform.rotation, // Используем текущий поворот объекта
            Vector3.one
        );

        Gizmos.matrix = matrix;

        // Отрисовываем куб
        Gizmos.DrawWireCube(Vector3.zero, collisionCubeSize);
    }

}
