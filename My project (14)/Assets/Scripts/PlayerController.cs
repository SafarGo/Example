using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 5f;
    public Transform cameraTransform;
    public LayerMask groundMask;

    private Rigidbody rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Блокируем вращение RigidBody
    }

    void Update()
    {
        // Проверка на землю
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f, groundMask);

        // Получаем ввод с клавиатуры
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Определяем направление движения относительно камеры
        Vector3 moveDirection = (cameraTransform.forward * moveZ + cameraTransform.right * moveX).normalized;
        moveDirection.y = 0f; // Не учитываем вертикальную составляющую

        // Перемещение персонажа
        Vector3 velocity = moveDirection * speed;
        rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);

        // Прыжок
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
