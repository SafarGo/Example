using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideCameraController : MonoBehaviour
{
    public float panSpeed = 20f; // Скорость перемещения камеры
    public Vector2 panLimitX = new Vector2(-50f, 50f); // Ограничения по оси X
    public Vector2 panLimitZ = new Vector2(-50f, 50f); // Ограничения по оси Z
    private Vector3 lastTouchPosition; // Последняя позиция касания
    private bool isDragging = false; // Флаг, показывающий, что идет перемещение

    void Update()
    {
        // Проверка для ПК: нажата левая кнопка мыши
        if (Input.GetMouseButtonDown(0))
        {
            lastTouchPosition = Input.mousePosition;
            isDragging = true;
        }

        // Проверка для ПК: отпущена левая кнопка мыши
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        // Проверка для мобильных устройств: одно касание
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                lastTouchPosition = touch.position;
                isDragging = true;
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isDragging = false;
            }
        }

        // Перемещение камеры
        if (isDragging)
        {
            Vector3 touchPosition = Input.touchCount > 0 ? (Vector3)Input.GetTouch(0).position : Input.mousePosition;
            Vector3 direction = lastTouchPosition - touchPosition;

            // Перемещаем камеру по X и Z, в зависимости от свайпа
            Vector3 move = new Vector3(direction.x, 0, direction.y) * panSpeed * Time.deltaTime;
            transform.Translate(move, Space.World);

            // Ограничиваем перемещение камеры по X
            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, panLimitX.x, panLimitX.y),
                transform.position.y,
                Mathf.Clamp(transform.position.z, panLimitZ.x, panLimitZ.y)
            );

            // Обновляем последнюю позицию касания
            lastTouchPosition = touchPosition;
        }
    }
}
