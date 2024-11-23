using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesDestroyer : MonoBehaviour
{
    [SerializeField] private LayerMask selectableLayer; // Слой для рейкаста
    [SerializeField] private float holdTime = 5f;       // Время удержания для удаления

    private float holdTimer;                            // Таймер удержания
    private GameObject targetObject;                   // Целевой объект рейкаста

    void Update()
    {
        // Рейкаст при нажатии ЛКМ
        if (Input.GetMouseButtonDown(0))
            StartRaycast();

        // Удержание ЛКМ
        if (Input.GetMouseButton(0) && targetObject)
            CheckHold();

        // Сброс, если отпущена ЛКМ
        if (Input.GetMouseButtonUp(0))
            ResetTarget();
    }

    private void StartRaycast()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),
            out RaycastHit hit, Mathf.Infinity, selectableLayer)
            && hit.collider.CompareTag("cantDestroyObstacle"))
        {
            targetObject = hit.collider.gameObject;
            holdTimer = 0f; // Сброс таймера
        }
    }

    private void CheckHold()
    {
        holdTimer += Time.deltaTime;
        if (holdTimer >= holdTime)
        {
            Destroy(targetObject); // Удаление объекта
            ResetTarget();         // Сброс
        }
    }

    private void ResetTarget()
    {
        targetObject = null;
        holdTimer = 0f;
    }
}
