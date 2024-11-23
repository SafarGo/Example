using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    public float raycastLength = 10f; // ����� ��������
    public Transform startPoint; // ��������� ����� (����� ������� � ������� ��������)
    public Transform targetPoint; // ������� ����� (����� ������� � ������� ��������)
    public GameObject itemPrefab; // ������ ��� �������� ����
    public float itemSpacing = 1f; // ���������� ����� ��������� �� ����

    private Camera mainCamera;
    private bool isStartPointSelected = false;
    private bool isTargetPointSelected = false;
    private bool pathSpawned = false; // ����, ����� �������� ���� ������ ���� ���

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        // ��������� ����� ����� � ������� �������� � ������� "T"
        if (Input.GetKeyDown(KeyCode.T))
        {
            TrySelectPoint();
        }

        // ���� ��� ����� ������� � ���� ��� �� ��� ������, ������������ ����
        if (isStartPointSelected && isTargetPointSelected && !pathSpawned)
        {
            // ������� ������� ����� ����
            SpawnItemsBetweenTransforms(startPoint, targetPoint);
            pathSpawned = true; // ������������� ����, ����� ���� ������ �� ����������
        }
    }

    void TrySelectPoint()
    {
        // ������� �� ������ � ����������� �������
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastLength))
        {
            // ���������, ��� �� ������ � ������, � ������� ����� ��������
            if (!isStartPointSelected)
            {
                // �������� ��������� �����
                startPoint = hit.transform;
                isStartPointSelected = true;
                Debug.Log("Start point selected");
            }
            else if (!isTargetPointSelected)
            {
                // �������� ������� �����
                targetPoint = hit.transform;
                isTargetPointSelected = true;
                Debug.Log("Target point selected");
            }
            else
            {
                // ���� ��� ������� ��� �����, ���������� ��, ���� ������� �� ����� � ������
                startPoint = null;
                targetPoint = null;
                isStartPointSelected = false;
                isTargetPointSelected = false;
                pathSpawned = false; // ���������� ����, ����� ���� ��� ���� ����� ��������
                Debug.Log("Points reset");
            }
        }
        else
        {
            // ���� ������� �� ����� � ������, ���������� �����
            startPoint = null;
            targetPoint = null;
            isStartPointSelected = false;
            isTargetPointSelected = false;
            pathSpawned = false; // ���������� ����, ����� ���� ��� ���� ����� ��������
            Debug.Log("Points reset");
        }
    }

    void SpawnItemsBetweenTransforms(Transform start, Transform target)
    {
        // ���� ����� �� �������, �� ������ ������
        if (start == null || target == null) return;

        // ���������� ������� �� ��� Y, �������� � ��� ����� �����
        Vector3 startPosition = new Vector3(start.position.x, 0, start.position.z);
        Vector3 targetPosition = new Vector3(target.position.x, 0, target.position.z);

        // ��������� ����������� �� ��������� ����� � �������
        Vector3 direction = (targetPosition - startPosition).normalized;

        // ��������� ���������� ����� �������
        float distance = Vector3.Distance(startPosition, targetPosition);

        // ���������� ���������� �������� ��� ������ ����� ����
        int itemCount = Mathf.FloorToInt(distance / itemSpacing);

        for (int i = 0; i < itemCount; i++)
        {
            // ��������� ������� ������� �������� ����� ����
            Vector3 spawnPosition = startPosition + direction * i * itemSpacing;

            // ������� ������
            GameObject item = Instantiate(itemPrefab, spawnPosition, Quaternion.identity);

            // ������������ ������ � ������� ��������� �����
            Vector3 lookDirection = (startPosition - spawnPosition).normalized;

            // ������� ������� �� ��� Y � ������� ������� �����
            item.transform.rotation = Quaternion.LookRotation(lookDirection);
        }

        Debug.Log("Path spawned");
    }
}
