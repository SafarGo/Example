using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoLOD : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private Plane[] cameraFrustumPlanes;

    private void Start()
    {
        // Кэшируем MeshRenderer для оптимизации
        meshRenderer = GetComponent<MeshRenderer>();

        if (meshRenderer == null)
        {
            Debug.LogWarning("У объекта отсутствует MeshRenderer!");
            enabled = false; // Отключаем скрипт, если MeshRenderer отсутствует
        }
    }

    private void FixedUpdate()
    {
        if (!(Camera.allCameras.Length > 0) || !meshRenderer)
            return;

        // Проверяем видимость объекта хотя бы одной камерой
        bool isVisible = false;
        foreach (Camera cam in Camera.allCameras)
        {
            if (!cam) continue;

            cameraFrustumPlanes = GeometryUtility.CalculateFrustumPlanes(cam);

            if (GeometryUtility.TestPlanesAABB(cameraFrustumPlanes, meshRenderer.bounds))
            {
                isVisible = true;
                break; // Если объект видим хотя бы одной камерой, выходим из цикла
            }
        }

        // Включаем или отключаем MeshRenderer
        meshRenderer.enabled = isVisible;
    }
}
