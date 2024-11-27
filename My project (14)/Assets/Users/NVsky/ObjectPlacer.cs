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

    public LayerMask collisionLayer; // ����, � ������� ����������� ������������

    private Renderer[] renderers;
    private Material[][] originalMaterials;
    private bool isRed = false;

    private Vector3 lastValidPosition;
    private Quaternion currentRotation = Quaternion.identity; // ������ ������� ������� �������

    private void Start()
    {
        mainCamera = Camera.main;
        targetPos = transform.position;

        renderers = GetComponentsInChildren<Renderer>();
        originalMaterials = new Material[renderers.Length][];

        for (int i = 0; i < renderers.Length; i++)
        {
            originalMaterials[i] = renderers[i].materials;
        }

        lastValidPosition = transform.position;

        // ������ ��������� ��������� �����, ���� ������ ����������� � ������ �������
        FindNearestEmptyPosition();
    }

    private void Update()
    {
        if (!isDragging) return;

        HandleRotation(); // ��������� ��������
        if (isDragging)
        {
            MoveObjectWithCollisions();
        }
    }

    private void OnMouseDown()
    {
        // ������ ��������������
        isDragging = true;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            offset = transform.position - hit.point;
            SetMaterialToColor(new Color(0.17f, 1, 0)); // ������� ���� ��� ������ ��������������
        }
    }

    private void OnMouseDrag()
    {
        // ���� �������������, ������ ����� ��������� �� �����
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

            if (!IsColliding(newPosition, currentRotation)) // ��������� ������� �������
            {
                targetPos = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * moveSpeed);
                lastValidPosition = targetPos;
                if (isRed)
                {
                    RestoreOriginalMaterials();
                    isRed = false;
                }
            }
            else
            {
                // ���� ���� ������������, ������ ���������� �������
                SetMaterialToColor(Color.red);
                isRed = true;
            }
        }
    }

    private void OnMouseUp()
    {
        // ��������� ��������������
        isDragging = false;

        if (!isRed)
        {
            lastValidPosition = targetPos;
        }

        RestoreOriginalMaterials();
    }

    private void HandleRotation()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            // ��������� �������� �� 90 �������� �� ��� Y
            currentRotation *= Quaternion.Euler(0, 90, 0); 
            transform.rotation = currentRotation;

            // ��������� �������� ����� ��������
            if (IsColliding(transform.position, currentRotation))
            {
                // ���� ���� ������������, ���������� ������ � ��������� ���������� �������
                transform.position = lastValidPosition;
                SetMaterialToColor(Color.red); // ���������� ������ ��� �������, ������������ � ��������
                isRed = true;
            }
            else
            {
                // ��������� ������� ��� ��������� ����������
                lastValidPosition = transform.position;
                RestoreOriginalMaterials();
                isRed = false;
            }
        }
    }

    private bool IsColliding(Vector3 newPosition, Quaternion rotation)
    {
        Collider[] colliders = Physics.OverlapBox(
            newPosition,
            GetComponent<Collider>().bounds.extents,
            rotation, // ��������� �������
            collisionLayer
        );
        foreach (Collider col in colliders)
        {
            if (col.gameObject != gameObject && !IsChildOfThisObject(col.gameObject))
            {
                return true; // ������������ ����������
            }
        }
        return false;
    }

    private bool IsChildOfThisObject(GameObject obj)
    {
        return obj.transform.IsChildOf(transform);
    }

    private void MoveObjectWithCollisions()
    {
        Vector3 newPosition = targetPos;

        // �������� �� ������������ � ������� ���������
        Collider[] colliders = Physics.OverlapBox(
            newPosition,
            GetComponent<Collider>().bounds.extents,
            currentRotation, // ��������� �������
            collisionLayer
        );
        foreach (Collider col in colliders)
        {
            if (col.gameObject != gameObject && !IsChildOfThisObject(col.gameObject))
            {
                // ���� ���� ������������, ���������� ������ � ��������� ���������� �������
                transform.position = lastValidPosition;
                return;
            }
        }

        // ���������� ������ � ����� �������
        transform.position = Vector3.MoveTowards(transform.position, newPosition, moveSpeed * Time.deltaTime);
    }

    private void SetMaterialToColor(Color color)
    {
        foreach (Renderer renderer in renderers)
        {
            Material[] newMaterials = new Material[renderer.materials.Length];
            for (int i = 0; i < renderer.materials.Length; i++)
            {
                newMaterials[i] = new Material(renderer.materials[i]) { color = color };
            }
            renderer.materials = newMaterials;
        }
    }

    private void RestoreOriginalMaterials()
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].materials = originalMaterials[i];
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

                if (!IsColliding(newPos, currentRotation)) // ��������� ������� �������
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
}
