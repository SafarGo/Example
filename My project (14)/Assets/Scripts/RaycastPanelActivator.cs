using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastPanelActivator : MonoBehaviour
{
    public float raycastDistance = 10f; // ��������� ��� Raycast
    public LayerMask layerMask; // ����� ��� �������� �������� � ����� Builder
    private Transform lastBuilderObject = null; // ��� �������� ���������� ������� Builder

    void Update()
    {
        // �������� �������, ����� ��������� ������� � ����� "Builder"
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, raycastDistance, layerMask))
        {
            if (hit.collider.CompareTag("Builder"))
            {
                Transform builderObject = hit.collider.transform;

                // ���������, ����� ��� ��� ����� ������
                if (lastBuilderObject != builderObject)
                {
                    // ��������� ������ ����������� �������, ���� ����
                    DeactivatePanel();

                    // ��������� ������� ������
                    lastBuilderObject = builderObject;

                    // ������� ������ � ����� �������
                    Transform panel = builderObject.Find("Panel");

                    if (panel != null)
                    {
                        // �������� ������
                        panel.gameObject.SetActive(true);
                    }
                    else
                    {
                        Debug.LogWarning("�������� ������ 'Panel' �� ������ � ������� " + builderObject.name);
                    }
                }
            }
        }
        else
        {
            // ���� ������ Builder �� � ���� ������, ��������� ������
            DeactivatePanel();
        }
    }

    // ����� ��� ���������� ������
    void DeactivatePanel()
    {
        if (lastBuilderObject != null)
        {
            Transform panel = lastBuilderObject.Find("Panel");
            if (panel != null)
            {
                panel.gameObject.SetActive(false);
            }
            lastBuilderObject = null;
        }
    }
}