using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SceneOptimizer : MonoBehaviour
{
    public Camera mainCamera; // Камера, определяющая зону видимости
    public LayerMask objectLayer; // Слой объектов для проверки видимости

    private Plane[] frustumPlanes; // Плоскости камеры

    private void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main; // Установка основной камеры по умолчанию
        }
    }

    private void LateUpdate()
    {
        // Обновляем плоскости видимости для текущей камеры
        frustumPlanes = GeometryUtility.CalculateFrustumPlanes(mainCamera);

        // Поиск только объектов на заданном слое
        Collider[] colliders = Physics.OverlapSphere(mainCamera.transform.position, mainCamera.farClipPlane, objectLayer);

        foreach (Collider collider in colliders)
        {
            GameObject obj = collider.gameObject;
            Renderer renderer = obj.GetComponent<Renderer>();
            VisualEffect vfx = obj.GetComponent<VisualEffect>();

            if (renderer != null && GeometryUtility.TestPlanesAABB(frustumPlanes, renderer.bounds))
            {
                // Включаем визуальные компоненты, если объект виден
                EnableComponents(renderer, vfx);
            }
            else
            {
                // Отключаем компоненты, если объект не виден
                DisableComponents(renderer, vfx);
            }
        }
    }

    private void EnableComponents(Renderer renderer, VisualEffect vfx)
    {
        if (renderer != null) renderer.enabled = true;
        if (vfx != null) vfx.enabled = true;
    }

    private void DisableComponents(Renderer renderer, VisualEffect vfx)
    {
        if (renderer != null) renderer.enabled = false;
        if (vfx != null) vfx.enabled = false;
    }
}
