using System.Collections.Generic;
using UnityEngine;

public class BuildingSystem : MonoBehaviour
{
    [Header("Building Settings")]
    public LayerMask groundLayer; // Слой земли, куда можно строить
    public LayerMask collisionLayer; // Слой объектов, с которыми нужно проверять столкновения
    public float gridSize = 1f; // Размер сетки для привязки
    public Color validPlacementColor = Color.green; // Цвет объекта, если место подходящее
    public Color invalidPlacementColor = Color.red; // Цвет объекта, если место неподходящее
    public float rayDistance = 100f; // Дистанция луча для проверки позиции на земле

    [Header("Movement Settings")]
    public float moveSpeed = 10f; // Скорость плавного перемещения объекта
    public bool alignToSurface = true; // Выравнивать объект по нормали поверхности

    [Header("Debug Settings")]
    public bool drawRay = true; // Отображать луч в редакторе

    private Renderer[] renderers; // Все рендереры объекта
    private Material[][] originalMaterials; // Оригинальные материалы для восстановления цвета
    private bool canPlace = true; // Можно ли размещать объект в текущей позиции
    private Camera mainCamera; // Камера игрока
    private Vector3 currentTargetPosition; // Целевая позиция объекта
    private Quaternion currentTargetRotation; // Целевое вращение объекта

    private void Start()
    {
        mainCamera = Camera.main;

        // Получаем все рендереры объекта
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
        // Проверка на наличие пересечений с другими объектами
        Collider[] colliders = Physics.OverlapBox(
            transform.position,
            GetComponent<Collider>().bounds.extents,
            transform.rotation,
            collisionLayer
        );

        // Устанавливаем возможность размещения объекта
        canPlace = colliders.Length == 0;

        // Меняем цвет объекта в зависимости от возможности размещения
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
