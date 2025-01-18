using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AutoLOD : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private Plane[] cameraFrustumPlanes;
    bool isDaleko;
    bool isVisible;
    private void Start()
    {
        // Кэшируем MeshRenderer для оптимизации
        meshRenderer = GetComponent<MeshRenderer>();

        if (meshRenderer == null)
        {
            Debug.LogWarning("У объекта отсутствует MeshRenderer!");
            enabled = false; // Отключаем скрипт, если MeshRenderer отсутствует
        }
        ///InvokeRepeating(nameof(CheckDistance), 2, 2);
    }

    private void FixedUpdate()
    {
        if (isDaleko == false)
        {
            if (!(Camera.allCameras.Length > 0) || !meshRenderer)
                return;

            // Проверяем видимость объекта хотя бы одной камерой
            isVisible = false;
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

    void CheckDistance()
    {
      // if (meshRenderer != null && isVisible)
      // {
      //     foreach (Camera cam in Camera.allCameras)
      //     {
      //         if (!cam) continue;
      //
      //         float distance = Vector3.Distance(gameObject.transform.position, cam.transform.position);
      //         if (distance > 900)
      //         {
      //             meshRenderer.enabled = false;
      //             isDaleko = true;
      //         }
      //         else
      //         {
      //             isDaleko = false;
      //             meshRenderer.enabled = true;
      //         }
      //
      //     }
      // }
    }
}
