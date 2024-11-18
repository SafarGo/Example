using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeController : MonoBehaviour
{
    public float conveyorSpeed = 5f; // Скорость движения конвейера
    public Vector3 conveyorDirection = Vector3.right; // Направление движения конвейера (по умолчанию по оси X)

    private void OnCollisionStay(Collision collision)
    {
        // Проверяем, есть ли у объекта RigidBody
        Rigidbody rb = collision.rigidbody;

        if (rb != null)
        {
            // Вычисляем направление движения по локальной оси X конвейера
            Vector3 moveDirection = transform.right; // Локальная ось X конвейера

            // Устанавливаем скорость объекта в направлении оси X конвейера
            rb.velocity = moveDirection * conveyorSpeed;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Когда объект покидает конвейер, возвращаем стандартную физику
        Rigidbody rb = collision.collider.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = true;
        }
    }
}
