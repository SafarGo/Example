using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    public bool allowMoveX = true; // ��������� �� ����������� �� X
    public bool allowMoveY = false; // ��������� �� ����������� �� Y
    public bool allowMoveZ = true; // ��������� �� ����������� �� Z
    public GridManager gridManager;

    private Camera mainCamera;
    private bool isDragging = false;
    private Vector3 offset;
    private Vector3 targetPosition; // �������, � ������� ������ ��������
    public float moveSpeed = 10f; // �������� �����������
    public float detectionRadius = 2f; // ������ ��� ������ ������ ��������
    public LayerMask collisionLayer; // ����, �� ������� ����� ������ �������

    private Renderer[] renderers; // ������ ��� ��������� ������ ���� Renderer'��
    private Color originalColor; // ������������ ���� �������
    private bool isRed = false; // ����, ����� �����������, ���� �� ������ �������
    private Vector3 positionWhenNotRed; // ������� �������, ����� �� �� ��� �������

    void Start()
    {
        mainCamera = Camera.main;
        targetPosition = transform.position; // ��������� ���� � ������� �������

        // ��������� �������� ������� �������
        positionWhenNotRed = transform.position;

        // �������� ��� ���������� Renderer ��� �������� ������� � ��� �������� ��������
        renderers = GetComponentsInChildren<Renderer>();

        if (renderers.Length > 0)
        {
            // ��������� ������������ ���� ������� ���������� Renderer
            originalColor = renderers[0].material.color;
        }
        else
        {
            Debug.LogWarning("No Renderer components found on " + gameObject.name + " or its children.");
        }
    }

    void Update()
    {
        // ���������� ���������� ������ � ������� �������
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // �������� �� ������ ������� ����������
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

            // ������������� ����� ������� �������
            targetPosition = newPosition;
        }
    }

    void OnMouseUp()
    {
        isDragging = false;

        // ���� ������ ���� �������, ���������� ��� �������, ��� �� ��� �� �������
        if (isRed)
        {
            targetPosition = positionWhenNotRed; // �������� �������� � �������� �������
            foreach (var renderer in renderers)
            {
                renderer.material.color = originalColor;
            }
        }
    }

    void CheckForNearbyObjects()
    {
        // �������� �� ������������ � ������� ��������� � �������
        Collider[] nearbyObjects = Physics.OverlapSphere(transform.position, detectionRadius, collisionLayer);

        foreach (var collider in nearbyObjects)
        {
            // ���������� ������������ � ����� �����
            if (collider.gameObject == gameObject)
                continue;

            // ���� ���� ������� �����, ������ ���� ���� Renderer'�� �� �������
            if (!isRed) // ���� ������ ��� �� ���� �������, �� ������ ��� ����
            {
                foreach (var renderer in renderers)
                {
                    renderer.material.color = Color.red;
                }
                isRed = true; // ������������� ����, ��� ������ ���� �������
                positionWhenNotRed = transform.position; // ���������� ������� �������, ����� ������ ���� �������
            }
        }

        // ���� �������� ��� ����� � ������ ���� �������, ��������������� �������� ����
        if (nearbyObjects.Length == 0 && isRed)
        {
            RestoreOriginalColor();
            isRed = false; // ������� ����, ��� ������ ���� �������
        }
    }

    void RestoreOriginalColor()
    {
        // ��������������� �������� ���� ���� Renderer'��
        foreach (var renderer in renderers)
        {
            renderer.material.color = originalColor;
        }
    }
}
