using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeController : MonoBehaviour
{
    [SerializeField] private float speed = 5f; // �������� �������� ��������
    [SerializeField] private float centerForce = 2f; // ����, ������������� ������� � ������ ���������

    private void OnCollisionStay(Collision collision)
    {
        // ���������, ���� �� � ������� RigidBody
        Rigidbody rb = collision.rigidbody;

        if (rb != null)
        {
            // ��������� ����������� �������� �� ��������� ��� X ���������
            Vector3 moveDirection = transform.right;

            // ������������ ������� ������� ������������ ����������� ��� Z ���������
            Vector3 localPosition = transform.InverseTransformPoint(collision.transform.position);
            float distanceFromCenter = localPosition.z; // ���������� �� ����������� ��� Z

            // ������������ ����, ������������� ������ � ������ ���������
            Vector3 centerCorrectionForce = -transform.forward * distanceFromCenter * centerForce;

            // �������� �������� �������: �� ��� X ��������� + ��������� � ������
            Vector3 finalVelocity = moveDirection * speed + centerCorrectionForce;

            // ������������� �������� �������
            rb.velocity = finalVelocity;
        }
    }
}
