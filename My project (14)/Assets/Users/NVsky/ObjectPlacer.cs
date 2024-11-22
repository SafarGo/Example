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

    }

    void Update()
    {

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (isDragging)
        {

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
