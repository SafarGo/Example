using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float speed = 5f; // Скорость движения персонажа
    public float jumpForce = 5f; // Сила прыжка
    public Transform cameraTransform; // Трансформ камеры
    public LayerMask groundMask; // Маска для определения земли

    private Rigidbody rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Замораживаем вращение RigidBody
    }

    void Update()
    {
        // Проверка на землю
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f, groundMask);

        // Получаем ввод с клавиатуры
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Определяем направление движения относительно камеры
        Vector3 direction = (cameraTransform.forward * moveZ + cameraTransform.right * moveX).normalized;
        direction.y = 0f; // Убираем влияние по оси Y, чтобы персонаж не двигался вверх

        // Рассчитываем скорость движения
        Vector3 velocity = direction * speed;
        rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);

        // Прыжок
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
