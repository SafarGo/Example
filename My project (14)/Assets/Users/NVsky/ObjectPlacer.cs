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

    public LayerMask collisionLayer; // ���� ��� ��������
    private Vector3 lastValidPosition;
    private Quaternion currentRotation = Quaternion.identity;
    [Header("ID ��� ������")]
    public int ObjectSpawnerID;

    public int ObjectID;

    private void Start()
    {
        mainCamera = Camera.main;
        targetPos = transform.position;

        lastValidPosition = transform.position;

        // �������� �� �������� ��� ������, ���� ������ ����������� � ������ �������
        ////////�����������, �� ����������/////FindNearestEmptyPosition();
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
                // ���� ��������, ��������� ������ � ������� ���������
                transform.position = lastValidPosition;
            }
        }
    }

    private void OnMouseUp()
    {
        StaticHolder.AllSpawnedObjectsTranforms[ObjectID] = gameObject.transform.position;
        StaticHolder.AllSpawnedObjectsRotations[ObjectID] = gameObject.transform.rotation;
        JsonSaver._instance.Save();
        Debug.Log("��������� ��� ������������");
        isDragging = false;
    }

    private void HandleRotation()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            currentRotation *= Quaternion.Euler(0, 90, 0); // ������� �� 90 �������� �� ��� Y
            transform.rotation = currentRotation;

            if (IsColliding(transform.position, currentRotation))
            {
                // ���� ������������, ��������� � ��������� ���������� �������
                transform.position = lastValidPosition;
            }
            else
            {
                lastValidPosition = transform.position; // ��������� ��������� ���������� �������
            }
        }
    }

    private bool IsColliding(Vector3 newPosition, Quaternion rotation)
    {
        Collider[] colliders = Physics.OverlapBox(newPosition, GetComponent<Collider>().bounds.extents, rotation, collisionLayer);

        foreach (Collider col in colliders)
        {
            // ������������ ������������ � ����� � ��������� ���������
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

        // �������� �� ������������ � ������� ���������
        if (IsColliding(newPosition, currentRotation))
        {
            transform.position = lastValidPosition; // ������� � ��������� ���������� �������, ���� ������������
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, newPosition, moveSpeed * Time.deltaTime);
        }
    }

    // ����� ��� ������ ���������� ���������� �����
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

    // ���������� OnCollisionStay ��� OnTriggerStay ��� ����� ������ ��������� ������������
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("obstacle_tag"))
        {
            // ��������� ������������ � ��������� � ����� obstacle_tag
            Debug.Log("������������ � ��������: " + collision.gameObject.name);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("obstacle_tag"))
        {
            // ��������� ������������ � ��������� � ����� obstacle_tag
            Debug.Log("������� � ��������: " + other.gameObject.name);
        }
    }
}
