using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeController : MonoBehaviour
{
    [SerializeField] private float speed = 5f; // Скорость движения объектов
    [SerializeField] private float centerForce = 2f; // Сила, притягивающая объекты к центру конвейера

    private void OnCollisionStay(Collision collision)
    {
        // Проверяем, есть ли у объекта RigidBody
        Rigidbody rb = collision.rigidbody;

        if (rb != null)
        {
            // Вычисляем направление движения по локальной оси X конвейера
            Vector3 moveDirection = transform.right;

            // Рассчитываем позицию объекта относительно центральной оси Z конвейера
            Vector3 localPosition = transform.InverseTransformPoint(collision.transform.position);
            float distanceFromCenter = localPosition.z; // Расстояние от центральной оси Z

            // Рассчитываем силу, притягивающую объект к центру конвейера
            Vector3 centerCorrectionForce = -transform.forward * distanceFromCenter * centerForce;

            // Итоговое движение объекта: по оси X конвейера + коррекция к центру
            Vector3 finalVelocity = moveDirection * speed + centerCorrectionForce;

            // Устанавливаем скорость объекта
            rb.velocity = finalVelocity;
        }
    }
}
