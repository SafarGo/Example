using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class CarMovementController : MonoBehaviour
{

    public float moveSpeed = 10f;
    public float acceleration = 5f;
    public float deceleration = 5f;
    public float turnSpeed = 100f;
    public float leanAmount = 10f;
    public Transform wheel;
    public float wheelRotationSpeed = 500f;
    public Rigidbody rb;
    public float tiltSpeed = 5f;
    public float jumpForce = 10f;
    public float groundCheckDistance = 1f;
    public float groundAttractionForce = 50f;

    private float inputHorizontal;
    private float inputVertical;
    [SerializeField] private float currentSpeed = 0f;
    [SerializeField] private float cuurentFuel = 360;
    private float tiltAngle = 0f;
    private bool isGrounded = false;

    public GameObject Player;
    public GameObject Medved;
    public Camera followCamera;
    public Vector3 cameraOffset = new Vector3(0, 5, -10);
    public float cameraFollowSpeed = 5f;


    public float mouseSensitivity = 2f;
    public float verticalRotationLimit = 80f;
    private float currentVerticalAngle = 0f;
    private float currentHorizontalAngle = 0f;
    public bool isCanMove;

    public float cameraCollisionSmoothSpeed = 10f;
    public float minimumCameraDistance = 2f;

    private AudioSource engineSound;

    [Header("Проскальзывание")]
    public float slipAmount = 1f; // Коэффициент бокового проскальзывания
    public float stopFriction = 5f; // Трение при остановке
    public float slipDamping = 0.95f; // Уменьшение проскальзывания с течением времени

    private Vector3 slipVelocity = Vector3.zero; // Текущее проскальзывание
    void Start()
    {
        engineSound = followCamera.gameObject.GetComponent<AudioSource>();
        isCanMove = false;
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }


        rb.mass = 1f;
        rb.drag = 1f;
        rb.angularDrag = 1f;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    void Update()
    {
        if (isCanMove == true)
        {

            inputHorizontal = Input.GetAxis("Horizontal");
            inputVertical = Input.GetAxis("Vertical");


            TurnMonocycle();


            RotateWheel();


            Debug.Log("Is Grounded: " + isGrounded);

            // Следование камеры
            FollowCamera();

            // Поворот камеры с помощью мыши
            RotateCameraWithMouse();
            if (Input.GetKeyDown(KeyCode.R))
            {
                followCamera.gameObject.SetActive(false);
                Player.transform.position = gameObject.transform.position;
                Player.SetActive(true);
                isCanMove = false;

            }

            //if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
            //{
            //    engineSound.pitch = currentSpeed + 0.5f;
            //}
            //else
            //{
            //    if (engineSound.pitch >= 1)
            //        engineSound.pitch -= Time.deltaTime / 2;
            //}
            UpdateEngineSoundPitch();
            Medved.SetActive(true);
        }
        else
        {
            Medved.SetActive(false);
        }

    }

    void FixedUpdate()
    {
        if (isCanMove == true)
        {
            if (isGrounded)
            {
                if (cuurentFuel > 0)
                    MoveMonocycle();
                else
                    currentSpeed = 0;
            }
            ApplyGroundAttraction();
            CorrectLean();
            CheckForJump();

            // Добавляем эффект проскальзывания
            ApplySlip();
        }
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
        //if (collision.collider.CompareTag("Ground"))
        //{
            isGrounded = true;
        //}
    }

    // Этот метод будет вызван, когда объект продолжает сталкиваться с чем-то
    private void OnCollisionStay(Collision collision)
    {
        //if (collision.collider.CompareTag("Ground"))
        //{
            isGrounded = true;
        //}
    }

    // Этот метод будет вызван, когда объект перестанет сталкиваться с чем-то
    private void OnCollisionExit(Collision collision)
    {
        //if (collision.collider.CompareTag("Ground"))
        //{
            isGrounded = false;
        //}
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

    void ApplySlip()
    {
        if (isGrounded)
        {
            // Рассчитываем боковую скорость
            Vector3 localVelocity = transform.InverseTransformDirection(rb.velocity);
            float lateralSpeed = localVelocity.x;

            // Добавляем боковую силу для проскальзывания при поворотах
            if (Mathf.Abs(inputHorizontal) > 0.1f)
            {
                slipVelocity += transform.right * lateralSpeed * slipAmount * Time.fixedDeltaTime;
            }

            // Уменьшаем проскальзывание с течением времени
            slipVelocity *= slipDamping;

            // Применяем боковую силу к Rigidbody
            rb.AddForce(-slipVelocity, ForceMode.VelocityChange);

            // Добавляем трение при полной остановке
            if (Mathf.Abs(currentSpeed) < 0.1f && inputVertical == 0)
            {
                rb.AddForce(-rb.velocity * stopFriction * Time.fixedDeltaTime, ForceMode.VelocityChange);
            }
        }
    }

    void UpdateEngineSoundPitch()
    {
        // Рассчитываем абсолютную скорость
        float absoluteSpeed = Mathf.Abs(currentSpeed);

        if (absoluteSpeed > 0.1f && cuurentFuel > 0)
        {
            // Увеличиваем питч при движении (вперёд или назад)
            engineSound.pitch = Mathf.Lerp(engineSound.pitch, 1f + (absoluteSpeed / moveSpeed) * 2f, Time.deltaTime);
            cuurentFuel -= Time.deltaTime;
        }
        else
        {
            // Плавно уменьшаем питч при остановке
            engineSound.pitch = Mathf.Lerp(engineSound.pitch, 1f, Time.deltaTime * 2f);
        }
    }
}
