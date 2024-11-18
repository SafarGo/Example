using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeController : MonoBehaviour
{
    public float conveyorSpeed = 5f; // �������� �������� ���������
    public Vector3 conveyorDirection = Vector3.right; // ����������� �������� ��������� (�� ��������� �� ��� X)

    private void OnCollisionStay(Collision collision)
    {
        // ���������, ���� �� � ������� RigidBody
        Rigidbody rb = collision.rigidbody;

        if (rb != null)
        {
            // ��������� ����������� �������� �� ��������� ��� X ���������
            Vector3 moveDirection = transform.right; // ��������� ��� X ���������

            // ������������� �������� ������� � ����������� ��� X ���������
            rb.velocity = moveDirection * conveyorSpeed;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // ����� ������ �������� ��������, ���������� ����������� ������
        Rigidbody rb = collision.collider.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = true;
        }
    }
}
