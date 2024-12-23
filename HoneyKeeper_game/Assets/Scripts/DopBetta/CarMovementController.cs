using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class CarMovementController : MonoBehaviour
{
    
    public float moveSpeed = 10f; // Максимальная скорость
    public float acceleration = 5f; // Ускорение
    public float deceleration = 5f; // Тормозное ускорение
    public float turnSpeed = 100f; // Скорость поворота
    public float leanAmount = 10f; // Угол наклона при повороте
    public Transform wheel; // Трансформ колеса
    public float wheelRotationSpeed = 500f; // Скорость вращения колеса
    public Rigidbody rb; // Rigidbody моноколеса
    public float tiltSpeed = 5f; // Скорость наклона для выпрямления
    public float jumpForce = 10f; // Сила прыжка для трамплинов
    public float groundCheckDistance = 1f; // Дистанция проверки, на земле ли моноколесо
    public float groundAttractionForce = 50f; // Сила притяжения к земле

    private float inputHorizontal;
    private float inputVertical;
    private float currentSpeed = 0f; // Текущая скорость моноколеса
    private float tiltAngle = 0f; // Текущий угол наклона
    private bool isGrounded = false; // Флаг, указывающий, находится ли моноколесо на земле

    public GameObject Player; // Камера, которая будет следовать за моноколесом
    public Camera followCamera; // Камера, которая будет следовать за моноколесом
    public Vector3 cameraOffset = new Vector3(0, 5, -10); // Смещение камеры относительно моноколеса
    public float cameraFollowSpeed = 5f; // Скорость следования камеры за моноколесом

    // Переменные для управления поворотом камеры с помощью мыши
    public float mouseSensitivity = 2f; // Чувствительность мыши
    public float verticalRotationLimit = 80f; // Ограничение по вертикали для камеры
    private float currentVerticalAngle = 0f; // Текущий угол по вертикали
    private float currentHorizontalAngle = 0f; // Текущий угол по горизонтали
    public bool isCanMove;

    public float cameraCollisionSmoothSpeed = 10f; // Скорость сглаживания при столкновении с объектом
    public float minimumCameraDistance = 2f; // Минимальное расстояние между камерой и моноколесом

    void Start()
    {
        isCanMove = false;
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }

        // Настройка Rigidbody
        rb.mass = 1f;  // Масса моноколеса
        rb.drag = 1f;  // Трение
        rb.angularDrag = 1f;  // Угловое трение
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous; // Включение детекции коллизий
        rb.interpolation = RigidbodyInterpolation.Interpolate;  // Плавное движение
    }

    void Update()
    {
        if (isCanMove == true)
        {
            // Получаем ввод от игрока
            inputHorizontal = Input.GetAxis("Horizontal");
            inputVertical = Input.GetAxis("Vertical");

            // Поворот моноколеса
            TurnMonocycle();

            // Вращение колеса в зависимости от скорости
            RotateWheel();

            // Отображение состояния (на земле или нет) в редакторе
            Debug.Log("Is Grounded: " + isGrounded);

            // Следование камеры
            FollowCamera();

            // Поворот камеры с помощью мыши
            RotateCameraWithMouse();
            if(Input.GetKeyDown(KeyCode.R))
            {
                followCamera.gameObject.SetActive(false);
                Player.transform.position = gameObject.transform.position;
                Player.SetActive(true);
                isCanMove = false;

            }
        }

    }

    void FixedUpdate()
    {
        // Движение моноколеса вперед/назад только если оно на земле
        if (isGrounded)
        {
            MoveMonocycle();
        }

        // Притягиваем моноколесо к земле, если оно слишком высоко
        ApplyGroundAttraction();

        // Стремление к выпрямлению
        CorrectLean();

        // Проверка столкновений с трамплинами
        CheckForJump();
    }

    void MoveMonocycle()
    {
        // Управление разгона и тормоза
        if (inputVertical > 0)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, moveSpeed, acceleration * Time.fixedDeltaTime);
        }
        else if (inputVertical < 0)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, -moveSpeed, acceleration * Time.fixedDeltaTime);
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, deceleration * Time.fixedDeltaTime);
        }

        // Двигаем моноколесо вперед или назад с помощью силы
        Vector3 moveDirection = transform.forward * currentSpeed;
        rb.AddForce(moveDirection, ForceMode.VelocityChange);
    }

    void TurnMonocycle()
    {
        // Поворот моноколеса влево/вправ
        float turn = inputHorizontal * turnSpeed * Time.deltaTime;
        transform.Rotate(0, turn, 0);

        // Добавляем наклон моноколеса при повороте
        if (inputHorizontal != 0)
        {
            tiltAngle = Mathf.Lerp(tiltAngle, -inputHorizontal * leanAmount, Time.deltaTime * tiltSpeed);
        }
        else
        {
            tiltAngle = Mathf.Lerp(tiltAngle, 0, Time.deltaTime * tiltSpeed);
        }

        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, tiltAngle);
    }

    void RotateWheel()
    {
        // Вращаем колесо в зависимости от скорости моноколеса
        float wheelRotation = currentSpeed * wheelRotationSpeed * Time.deltaTime;
        wheel.Rotate(wheelRotation, 0, 0);
    }

    void CorrectLean()
    {
        // Стремление моноколеса к выпрямлению, если оно не наклоняется
        if (Mathf.Abs(inputHorizontal) < 0.1f)
        {
            tiltAngle = Mathf.Lerp(tiltAngle, 0, Time.fixedDeltaTime * tiltSpeed);
        }
    }

    void CheckForJump()
    {
        // Если моноколесо сталкивается с объектом (например, трамплином) и движется вверх
        if (isGrounded && rb.velocity.y <= 0)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 1f))
            {
                if (hit.collider.CompareTag("Ramp")) // Если это трамплин
                {
                    rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // Подлетаем
                }
            }
        }
    }

    // Этот метод будет вызван, когда объект столкнется с чем-то
    private void OnCollisionEnter(Collision collision)
    {
        // Если моноколесо столкнулось с землей
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    // Этот метод будет вызван, когда объект продолжает сталкиваться с чем-то
    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    // Этот метод будет вызван, когда объект перестанет сталкиваться с чем-то
    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    // Применяем силу для притяжения моноколеса к земле
    void ApplyGroundAttraction()
    {
        // Если моноколесо не на земле, мы применяем силу вниз
        if (!isGrounded)
        {
            rb.AddForce(Vector3.down * groundAttractionForce, ForceMode.Acceleration);
        }
    }

    // Метод для следования камеры за моноколесом
    void FollowCamera()
    {
        if (followCamera != null)
        {
            // Рассчитываем желаемую позицию камеры на основе смещения
            Vector3 desiredPosition = transform.position + cameraOffset;

            // Проверяем столкновение камеры с объектами
            RaycastHit hit;
            if (Physics.Raycast(transform.position, followCamera.transform.position - transform.position, out hit, cameraOffset.magnitude))
            {
                // Если столкновение произошло, приближаем камеру
                float distance = Vector3.Distance(transform.position, hit.point);
                desiredPosition = transform.position + (followCamera.transform.position - transform.position).normalized * Mathf.Max(distance, minimumCameraDistance);
            }

            // Плавное перемещение камеры к желаемой позиции
            Vector3 smoothedPosition = Vector3.Lerp(followCamera.transform.position, desiredPosition, cameraCollisionSmoothSpeed * Time.deltaTime);
            followCamera.transform.position = smoothedPosition;

            // Камера всегда смотрит на моноколесо
            followCamera.transform.LookAt(transform);
        }
    }

        // Метод для поворота камеры с помощью мыши
        void RotateCameraWithMouse()
    {
        if (followCamera != null)
        {
            // Получаем движение мыши
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            // Обновляем горизонтальный угол (поворот вокруг оси Y)
            currentHorizontalAngle += mouseX;

            // Обновляем вертикальный угол (поворот вокруг оси X) с ограничением
            currentVerticalAngle -= mouseY;
            currentVerticalAngle = Mathf.Clamp(currentVerticalAngle, -verticalRotationLimit, verticalRotationLimit);

            // Применяем углы для вращения камеры
            Quaternion rotation = Quaternion.Euler(currentVerticalAngle, currentHorizontalAngle, 0);
            followCamera.transform.position = transform.position + rotation * cameraOffset;

            // Камера смотрит на моноколесо
            followCamera.transform.LookAt(transform);
        }
    }
}
