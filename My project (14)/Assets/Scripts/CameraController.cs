using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody; // Персонаж, за которым будет следовать камера
    private float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Захватываем курсор
    }

    void Update()
    {
        // Получаем данные мыши
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Ограничиваем вращение по оси X
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Вращаем камеру вверх-вниз
        transform.localRotation = Quaternion.Euler(xRotation, 90f, 0f);

        // Вращаем персонажа влево-вправо
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
