using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesDestroyer : MonoBehaviour
{
    [SerializeField] private LayerMask selectableLayer; // ���� ��� ��������
    [SerializeField] private float holdTime = 5f;       // ����� ��������� ��� ��������

    private float holdTimer;                            // ������ ���������
    private GameObject targetObject;                   // ������� ������ ��������

    void Update()
    {
        // ������� ��� ������� ���
        if (Input.GetMouseButtonDown(0))
            StartRaycast();

        // ��������� ���
        if (Input.GetMouseButton(0) && targetObject)
            CheckHold();

        // �����, ���� �������� ���
        if (Input.GetMouseButtonUp(0))
            ResetTarget();
    }

    private void StartRaycast()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),
            out RaycastHit hit, Mathf.Infinity, selectableLayer)
            && hit.collider.CompareTag("cantDestroyObstacle"))
        {
            targetObject = hit.collider.gameObject;
            holdTimer = 0f; // ����� �������
        }
    }

    private void CheckHold()
    {
        holdTimer += Time.deltaTime;
        if (holdTimer >= holdTime)
        {
            Destroy(targetObject); // �������� �������
            ResetTarget();         // �����
        }
    }

    private void ResetTarget()
    {
        targetObject = null;
        holdTimer = 0f;
    }
}
