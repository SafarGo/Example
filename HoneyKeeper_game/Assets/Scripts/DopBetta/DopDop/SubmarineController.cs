using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float forwardSpeed = 10f; // Скорость движения вперёд
    public float rotationSpeed = 50f; // Скорость поворота
    public float verticalSpeed = 5f; // Скорость подъёма/опускания
    public float stopSmoothness = 2f; // Плавность остановки

    [Header("Tilt Settings")]
    public float tiltAngle = 30f; // Максимальный угол наклона
    public float tiltSmoothness = 5f; // Скорость изменения наклона

    [Header("Water Detection")]
    public string waterTag = "NaturalWaterReservoir"; // Тег воды

    private Rigidbody rb;
    public bool isInWater = false; // Находится ли подлодка в воде
    private Vector3 targetVelocity; // Целевая скорость для плавной остановки
    private Quaternion targetRotation; // Целевой наклон

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        //rb.useGravity = false; // Отключаем гравитацию
    }

    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, входит ли подлодка в воду
        if (other.CompareTag(waterTag))
        {
            rb.useGravity = false;
            isInWater = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Проверяем, выходит ли подлодка из воды
        if (other.CompareTag(waterTag))
        {
            isInWater = false;
            rb.useGravity = true;
        }
    }

    private void FixedUpdate()
    {
        // Движение работает только в воде
        if (!isInWater)
        {
             rb.velocity = rb.velocity;///Vector3.Lerp(rb.velocity, Vector3.zero, Time.fixedDeltaTime * stopSmoothness);
            // rb.angularVelocity = Vector3.zero;///Vector3.Lerp(rb.angularVelocity, Vector3.zero, Time.fixedDeltaTime * stopSmoothness);
            return;
        }
        else
        {

            // Получаем ввод от пользователя
            float horizontalInput = Input.GetAxis("Horizontal"); // A и D
            float verticalInput = Input.GetAxis("Vertical"); // W и S
            float verticalMovement = 0f;

            // Проверяем подъём/опускание
            if (Input.GetKey(KeyCode.Space)) // Пробел
            {
                verticalMovement = 1f;
            }
            else if (Input.GetKey(KeyCode.LeftShift)) // Левый Shift
            {
                verticalMovement = -1f;
            }

            // Движение вперёд/назад
            Vector3 forwardMovement = transform.forward * verticalInput * forwardSpeed;

            // Подъём/опускание
            Vector3 verticalMovementVector = Vector3.up * verticalMovement * verticalSpeed;

            // Устанавливаем целевую скорость
            targetVelocity = forwardMovement + verticalMovementVector;

            // Применяем скорость через Rigidbody
            rb.velocity = Vector3.Lerp(rb.velocity, targetVelocity, Time.fixedDeltaTime * stopSmoothness);

            // Поворот влево/вправо
            float rotation = horizontalInput * rotationSpeed * Time.fixedDeltaTime;
            rb.angularVelocity = new Vector3(0, rotation, 0);

            // Обновляем наклон
            UpdateTilt(verticalMovement);
        }
        
    }

    private void UpdateTilt(float verticalMovement)
    {
        // Вычисляем целевой угол наклона
        float tilt = -verticalMovement * tiltAngle;

        // Преобразуем в кватернион
        targetRotation = Quaternion.Euler(tilt, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

        // Плавно применяем наклон
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.fixedDeltaTime * tiltSmoothness);
    }
}
