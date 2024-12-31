using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    [Header("Настройки воды")]
    [SerializeField] private float pushForce = 10f; // Сила выталкивания
    [SerializeField] private float maxSpeed = 5f;   // Максимальная скорость объектов в воде

    private void OnTriggerStay(Collider other)
    {
        // Проверяем, есть ли Rigidbody у объекта
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Лог для отладки
            Debug.Log($"Объект {other.name} в воде!");

            // Направление выталкивающей силы - вверх
            Vector3 pushDirection = Vector3.up;

            // Применяем силу выталкивания
            rb.AddForce(pushDirection * pushForce * Time.deltaTime, ForceMode.Acceleration);

            // Ограничиваем максимальную скорость
            if (rb.velocity.magnitude > maxSpeed)
            {
                rb.velocity = rb.velocity.normalized * maxSpeed;
            }
        }
    }
}
