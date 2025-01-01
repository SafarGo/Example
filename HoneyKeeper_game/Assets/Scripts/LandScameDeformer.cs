using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandScameDeformer : MonoBehaviour
{
    //здесь меш продавливается public float deformationRadius = 0.5f; // Радиус области деформации
    //здесь меш продавливается public float deformationStrength = 0.1f; // Сила деформации
    //здесь меш продавливается 
    //здесь меш продавливается void OnCollisionEnter(Collision collision)
    //здесь меш продавливается {
    //здесь меш продавливается     // Получаем объект, с которым произошло столкновение
    //здесь меш продавливается     GameObject targetObject = collision.gameObject;
    //здесь меш продавливается 
    //здесь меш продавливается     // Проверяем, есть ли у объекта MeshFilter
    //здесь меш продавливается     MeshFilter targetMeshFilter = targetObject.GetComponent<MeshFilter>();
    //здесь меш продавливается     MeshCollider targetMeshCollider = targetObject.GetComponent<MeshCollider>();
    //здесь меш продавливается 
    //здесь меш продавливается     if (targetMeshFilter != null)
    //здесь меш продавливается     {
    //здесь меш продавливается         Mesh targetMesh = targetMeshFilter.mesh;
    //здесь меш продавливается 
    //здесь меш продавливается         // Проверяем, доступен ли меш для чтения/записи
    //здесь меш продавливается         if (!targetMesh.isReadable)
    //здесь меш продавливается         {
    //здесь меш продавливается             Debug.LogError($"Меш '{targetMesh.name}' не доступен для изменения. Включите Read/Write в настройках импорта.");
    //здесь меш продавливается             return;
    //здесь меш продавливается         }
    //здесь меш продавливается 
    //здесь меш продавливается         // Получаем вершины и нормали меша
    //здесь меш продавливается         Vector3[] vertices = targetMesh.vertices;
    //здесь меш продавливается         Vector3[] normals = targetMesh.normals;
    //здесь меш продавливается 
    //здесь меш продавливается         // Преобразуем позицию точки касания в локальные координаты цели
    //здесь меш продавливается         foreach (ContactPoint contact in collision.contacts)
    //здесь меш продавливается         {
    //здесь меш продавливается             Vector3 localContactPoint = targetObject.transform.InverseTransformPoint(contact.point);
    //здесь меш продавливается 
    //здесь меш продавливается             // Деформируем вершины в радиусе вокруг точки касания
    //здесь меш продавливается             for (int i = 0; i < vertices.Length; i++)
    //здесь меш продавливается             {
    //здесь меш продавливается                 float distance = Vector3.Distance(localContactPoint, vertices[i]);
    //здесь меш продавливается                 if (distance < deformationRadius)
    //здесь меш продавливается                 {
    //здесь меш продавливается                     float deformationAmount = Mathf.Lerp(deformationStrength, 0, distance / deformationRadius);
    //здесь меш продавливается                     vertices[i] -= normals[i] * deformationAmount; // Выгибаем вершину внутрь вдоль нормали
    //здесь меш продавливается                 }
    //здесь меш продавливается             }
    //здесь меш продавливается         }
    //здесь меш продавливается 
    //здесь меш продавливается         // Применяем измененные вершины к мешу
    //здесь меш продавливается         targetMesh.vertices = vertices;
    //здесь меш продавливается         targetMesh.RecalculateNormals(); // Пересчитываем нормали для корректного освещения
    //здесь меш продавливается         targetMesh.RecalculateBounds();  // Обновляем границы меша
    //здесь меш продавливается         targetMeshFilter.mesh = targetMesh;
    //здесь меш продавливается 
    //здесь меш продавливается         // Обновляем MeshCollider
    //здесь меш продавливается         if (targetMeshCollider != null)
    //здесь меш продавливается         {
    //здесь меш продавливается             targetMeshCollider.sharedMesh = null; // Сбрасываем старый меш
    //здесь меш продавливается             targetMeshCollider.sharedMesh = targetMesh; // Применяем обновленный меш
    //здесь меш продавливается         }
    //здесь меш продавливается     }
    //здесь меш продавливается }

    public float deformationRadius = 5f; // Радиус изменения
    public float deformationSpeed = 0.1f; // Скорость изменения
    public float targetHeightWorld = 10f; // Целевая высота в метрах

    private void OnCollisionEnter(Collision collision)
    {
        Terrain terrain = collision.gameObject.GetComponent<Terrain>();
        if (terrain == null) return;

        TerrainData terrainData = terrain.terrainData;

        // Преобразуем точку столкновения в локальные координаты террейна
        Vector3 collisionPoint = collision.contacts[0].point;
        Vector3 terrainLocalPos = collisionPoint - terrain.transform.position;

        // Нормализуем координаты относительно террейна (0..1)
        float normX = terrainLocalPos.x / terrainData.size.x;
        float normZ = terrainLocalPos.z / terrainData.size.z;

        // Преобразуем в индексы массива высот
        int heightMapX = Mathf.RoundToInt(normX * terrainData.heightmapResolution);
        int heightMapZ = Mathf.RoundToInt(normZ * terrainData.heightmapResolution);
        int radius = Mathf.RoundToInt(deformationRadius / terrainData.size.x * terrainData.heightmapResolution);

        // Преобразуем целевую высоту в нормализованное значение (0..1)
        float normalizedTargetHeight = targetHeightWorld / terrainData.size.y;

        // Получаем текущие высоты
        int size = radius * 2 + 1;
        float[,] heights = terrainData.GetHeights(
            Mathf.Clamp(heightMapX - radius, 0, terrainData.heightmapResolution - 1),
            Mathf.Clamp(heightMapZ - radius, 0, terrainData.heightmapResolution - 1),
            size,
            size
        );

        // Изменяем высоты
        for (int z = 0; z < size; z++)
        {
            for (int x = 0; x < size; x++)
            {
                float distance = Vector2.Distance(
                    new Vector2(x, z),
                    new Vector2(radius, radius)
                );

                if (distance <= radius)
                {
                    float currentHeight = heights[z, x];
                    heights[z, x] = Mathf.MoveTowards(currentHeight, normalizedTargetHeight, deformationSpeed * Time.deltaTime);
                }
            }
        }

        // Устанавливаем новые высоты
        terrainData.SetHeights(
            Mathf.Clamp(heightMapX - radius, 0, terrainData.heightmapResolution - 1),
            Mathf.Clamp(heightMapZ - radius, 0, terrainData.heightmapResolution - 1),
            heights
        );
    }
}
