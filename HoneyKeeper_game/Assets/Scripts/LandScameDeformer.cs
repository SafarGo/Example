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

    public float deformationRadius = 0.5f; // Радиус деформации
    public float deformationSpeed = 0.1f; // Скорость деформации
    public float targetHeightY = 1.0f; // Целевая высота по Y
    public float heightTolerance = 0.01f; // Погрешность для остановки

    private void Start()
    {
        Destroy(gameObject,0.5f);
    }
    void OnCollisionEnter(Collision collision)
    {
        // Получаем объект и проверяем наличие MeshFilter
        MeshFilter meshFilter = collision.gameObject.GetComponent<MeshFilter>();
        MeshCollider meshCollider = collision.gameObject.GetComponent<MeshCollider>();

        if (meshFilter == null || !meshFilter.mesh.isReadable)
            return;

        Mesh mesh = meshFilter.mesh;
        Vector3[] vertices = mesh.vertices;

        // Преобразуем контактные точки в локальные координаты меша
        Vector3 localContactPoint = collision.contacts[0].point;
        localContactPoint = collision.transform.InverseTransformPoint(localContactPoint);

        bool anyVertexChanged = false;

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 vertexWorldPos = collision.transform.TransformPoint(vertices[i]);

            // Проверяем только вершины в пределах радиуса
            if (Vector3.Distance(localContactPoint, vertices[i]) < deformationRadius)
            {
                // Если вершина еще не на целевой высоте
                if (Mathf.Abs(vertexWorldPos.y - targetHeightY) > heightTolerance)
                {
                    anyVertexChanged = true;

                    // Деформируем вершину только по Y
                    vertexWorldPos.y = Mathf.MoveTowards(vertexWorldPos.y, targetHeightY, deformationSpeed);
                    vertices[i] = collision.transform.InverseTransformPoint(vertexWorldPos);
                }
            }
        }

        // Если ни одна вершина не изменилась, прекращаем выполнение
        if (!anyVertexChanged)
        {
            Debug.Log("Все вершины в пределах радиуса выровнены. Деформация завершена.");
            return;
        }

        // Применяем изменения к мешу
        mesh.vertices = vertices;
        mesh.RecalculateNormals();

        // Обновляем MeshCollider, если он есть
        if (meshCollider != null)
        {
            meshCollider.sharedMesh = null;
            meshCollider.sharedMesh = mesh;
        }
    }
    private void Update()
    {
        transform.position += Vector3.back * 0.5f * Time.deltaTime;
    }
}
