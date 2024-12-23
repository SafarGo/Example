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
    public float groundCheckDistance = 1f; // Дистанция для проверки, на земле ли моноколесо

    private float inputHorizontal;
    private float inputVertical;
    private float currentSpeed = 0f; // Текущая скорость моноколеса
    private float tiltAngle = 0f; // Текущий угол наклона
    private bool isGrounded; // Флаг, указывающий, находится ли моноколесо на земле

    void Start()
    {
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
        // Получаем ввод от игрока
        inputHorizontal = Input.GetAxis("Horizontal");
        inputVertical = Input.GetAxis("Vertical");

        // Поворот моноколеса
        TurnMonocycle();

        // Вращение колеса в зависимости от скорости
        RotateWheel();

        // Проверка, находимся ли мы на земле
        CheckIfGrounded();

        // Отображение Raycast в редакторе
        Debug.DrawRay(transform.position, Vector3.down * groundCheckDistance, isGrounded ? Color.green : Color.red);
    }

    void FixedUpdate()
    {
        // Движение моноколеса вперед/назад только если оно на земле
        if (isGrounded)
        {
            MoveMonocycle();
        }

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
            if (Physics.Raycast(transform.position, Vector3.down, out hit, groundCheckDistance))
            {
                if (hit.collider.CompareTag("Ramp")) // Если это трамплин
                {
                    rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // Подлетаем
                }
            }
        }
    }

    // Проверка, что моноколесо на земле
    void CheckIfGrounded()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, groundCheckDistance))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
}
