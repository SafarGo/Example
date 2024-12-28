using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class CarMovementController : MonoBehaviour
{

    public float moveSpeed = 10f;
    public float acceleration = 15f; // Увеличено для быстрого разгона
    public float deceleration = 20f; // Увеличено для быстрого торможения
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
    [SerializeField] private float currentFuel = 360f;
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

    void Start()
    {
        engineSound = followCamera.gameObject.GetComponent<AudioSource>();
        isCanMove = false;
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }

        rb.mass = 1f;
        rb.drag = 0.5f; // Уменьшено сопротивление для более естественного движения
        rb.angularDrag = 1f;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    void Update()
    {
        if (isCanMove)
        {
            inputHorizontal = Input.GetAxis("Horizontal");
            inputVertical = Input.GetAxis("Vertical");

            TurnMonocycle();
            RotateWheel();
            FollowCamera();
            RotateCameraWithMouse();

            if (Input.GetKeyDown(KeyCode.R))
            {
                followCamera.gameObject.SetActive(false);
                Player.transform.position = transform.position;
                Player.SetActive(true);
                isCanMove = false;
            }

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
        if (isCanMove)
        {
            if (isGrounded)
            {
                MoveMonocycle();
            }
            ApplyGroundAttraction();
            CorrectLean();
        }
    }

    void MoveMonocycle()
    {
        // Управление разгоном и торможением
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

        // Применяем силу движения
        Vector3 moveDirection = transform.forward * currentSpeed;
        rb.velocity = new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z);

        // Увеличение сцепления на склонах
        if (isGrounded && Mathf.Abs(rb.velocity.y) > 0.1f)
        {
            rb.AddForce(-rb.velocity.normalized * deceleration, ForceMode.Acceleration);
        }
    }

    void TurnMonocycle()
    {
        float turn = inputHorizontal * turnSpeed * Time.deltaTime;
        transform.Rotate(0, turn, 0);

        // Добавляем наклон при поворотах
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
        float wheelRotation = currentSpeed * wheelRotationSpeed * Time.deltaTime;
        wheel.Rotate(wheelRotation, 0, 0);
    }

    void CorrectLean()
    {
        if (Mathf.Abs(inputHorizontal) < 0.1f)
        {
            tiltAngle = Mathf.Lerp(tiltAngle, 0, Time.fixedDeltaTime * tiltSpeed);
        }
    }

    void ApplyGroundAttraction()
    {
        if (!isGrounded)
        {
            rb.AddForce(Vector3.down * groundAttractionForce, ForceMode.Acceleration);
        }
    }

    void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
    }

    void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }

    void FollowCamera()
    {
        if (followCamera != null)
        {
            Vector3 desiredPosition = transform.position + cameraOffset;
            Vector3 smoothedPosition = Vector3.Lerp(followCamera.transform.position, desiredPosition, cameraFollowSpeed * Time.deltaTime);
            followCamera.transform.position = smoothedPosition;
            followCamera.transform.LookAt(transform);
        }
    }

    void RotateCameraWithMouse()
    {
        if (followCamera != null)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            currentHorizontalAngle += mouseX;
            currentVerticalAngle -= mouseY;
            currentVerticalAngle = Mathf.Clamp(currentVerticalAngle, -verticalRotationLimit, verticalRotationLimit);

            Quaternion rotation = Quaternion.Euler(currentVerticalAngle, currentHorizontalAngle, 0);
            followCamera.transform.position = transform.position + rotation * cameraOffset;
            followCamera.transform.LookAt(transform);
        }
    }

    void UpdateEngineSoundPitch()
    {
        float absoluteSpeed = Mathf.Abs(currentSpeed);

        if (absoluteSpeed > 0.1f && currentFuel > 0)
        {
            engineSound.pitch = Mathf.Lerp(engineSound.pitch, 1f + (absoluteSpeed / moveSpeed) * 2f, Time.deltaTime);
            currentFuel -= Time.deltaTime;
        }
        else
        {
            engineSound.pitch = Mathf.Lerp(engineSound.pitch, 1f, Time.deltaTime * 2f);
        }
    }
}
