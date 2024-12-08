using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class RaycastPanelActivator : MonoBehaviour
{
    public float raycastDistance = 10f; // Дистанция для Raycast
    public LayerMask layerMask; // Маска для проверки объектов с тегом Builder
    public LayerMask MigamelayerMask; 
    private Transform lastBuilderObject = null; // Для хранения последнего объекта Builder

    void Update()
    {
        // Проводим рейкаст, чтобы проверить объекты с тегом "Builder"
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, raycastDistance, layerMask))
        {
            if (hit.collider.CompareTag("Builder"))
            {
                Transform builderObject = hit.collider.transform;

                // Проверяем, чтобы это был новый объект
                if (lastBuilderObject != builderObject)
                {
                    // Выключаем панель предыдущего объекта, если есть
                    DeactivatePanel();

                    // Обновляем текущий объект
                    lastBuilderObject = builderObject;

                    // Находим панель в новом объекте
                    Transform panel = builderObject.Find("Panel");

                    if (panel != null)
                    {
                        // Включаем панель
                        panel.gameObject.SetActive(true);
                    }
                    else
                    {
                        Debug.LogWarning("Дочерний объект 'Panel' не найден в объекте " + builderObject.name);
                    }
                }
            }

        }
        else
        {
            // Если объект Builder не в поле зрения, выключаем панель
            DeactivatePanel();
        }
    }

        // Метод для отключения панели
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