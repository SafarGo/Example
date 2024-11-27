using System.Collections.Generic;
using UnityEngine;

public class BuildingSystem : MonoBehaviour
{
    [Header("Building Settings")]
    public LayerMask groundLayer; // ���� �����, ���� ����� �������
    public LayerMask collisionLayer; // ���� ��������, � �������� ����� ��������� ������������
    public float gridSize = 1f; // ������ ����� ��� ��������
    public Color validPlacementColor = Color.green; // ���� �������, ���� ����� ����������
    public Color invalidPlacementColor = Color.red; // ���� �������, ���� ����� ������������
    public float rayDistance = 100f; // ��������� ���� ��� �������� ������� �� �����

    [Header("Movement Settings")]
    public float moveSpeed = 10f; // �������� �������� ����������� �������
    public bool alignToSurface = true; // ����������� ������ �� ������� �����������

    [Header("Debug Settings")]
    public bool drawRay = true; // ���������� ��� � ���������

    private Renderer[] renderers; // ��� ��������� �������
    private Material[][] originalMaterials; // ������������ ��������� ��� �������������� �����
    private bool canPlace = true; // ����� �� ��������� ������ � ������� �������
    private Camera mainCamera; // ������ ������
    private Vector3 currentTargetPosition; // ������� ������� �������
    private Quaternion currentTargetRotation; // ������� �������� �������

    private void Start()
    {
        mainCamera = Camera.main;

        // �������� ��� ��������� �������
        renderers = GetComponentsInChildren<Renderer>();
        originalMaterials = new Material[renderers.Length][];
        for (int i = 0; i < renderers.Length; i++)
        {
            originalMaterials[i] = renderers[i].materials;
        }
    }

    private void Update()
    {
        CheckPlacementCollisions();
    }

    private void CheckPlacementCollisions()
    {
        // �������� �� ������� ����������� � ������� ���������
        Collider[] colliders = Physics.OverlapBox(
            transform.position,
            GetComponent<Collider>().bounds.extents,
            transform.rotation,
            collisionLayer
        );

        // ������������� ����������� ���������� �������
        canPlace = colliders.Length == 0;

        // ������ ���� ������� � ����������� �� ����������� ����������
        UpdateObjectColor(canPlace ? validPlacementColor : invalidPlacementColor);
    }

    private void UpdateObjectColor(Color color)
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
}
