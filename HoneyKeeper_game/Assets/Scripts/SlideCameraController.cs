using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideCameraController : MonoBehaviour
{
    public float panSpeed = 20f; // �������� ����������� ������
    public Vector2 panLimitX = new Vector2(-50f, 50f); // ����������� �� ��� X
    public Vector2 panLimitZ = new Vector2(-50f, 50f); // ����������� �� ��� Z
    private Vector3 lastTouchPosition; // ��������� ������� �������
    private bool isDragging = false; // ����, ������������, ��� ���� �����������

    void Update()
    {
        // �������� ��� ��: ������ ����� ������ ����
        if (Input.GetMouseButtonDown(0))
        {
            lastTouchPosition = Input.mousePosition;
            isDragging = true;
        }

        // �������� ��� ��: �������� ����� ������ ����
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        // �������� ��� ��������� ���������: ���� �������
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                lastTouchPosition = touch.position;
                isDragging = true;
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isDragging = false;
            }
        }

        // ����������� ������
        if (isDragging)
        {
            Vector3 touchPosition = Input.touchCount > 0 ? (Vector3)Input.GetTouch(0).position : Input.mousePosition;
            Vector3 direction = lastTouchPosition - touchPosition;

            // ���������� ������ �� X � Z, � ����������� �� ������
            Vector3 move = new Vector3(direction.x, 0, direction.y) * panSpeed * Time.deltaTime;
            transform.Translate(move, Space.World);

            // ������������ ����������� ������ �� X
            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, panLimitX.x, panLimitX.y),
                transform.position.y,
                Mathf.Clamp(transform.position.z, panLimitZ.x, panLimitZ.y)
            );

            // ��������� ��������� ������� �������
            lastTouchPosition = touchPosition;
        }
    }
}
