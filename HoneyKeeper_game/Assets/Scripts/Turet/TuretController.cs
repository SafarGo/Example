using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    [Header("Настройки управления")]
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private GameObject cameraToRotate;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject laser;

    private float rotationX = 0f;
    private float rotationY = 0f;

    private bool isPlayerHere;
    private bool isTurretActive;

    [Header("Настройки нагревания")]
    [SerializeField] private int maxTemperature = 100;
    [SerializeField] private float currentTemperature = 0f;

    private void Start()
    {
        if (!player) player = GameObject.FindWithTag("Player");
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Debug.Log($"Сейчас температура: {currentTemperature}");

        if (isPlayerHere && Input.GetKeyDown(KeyCode.E))
        {
            ToggleTurret();
        }

        if (isTurretActive)
        {
            HandleTurretControl();
        }
        else
        {
            UpdateTemperature(-Time.deltaTime);
        }
    }

    private void ToggleTurret()
    {
        isTurretActive = !isTurretActive;
        StaticHolder.isTurretActive = isTurretActive;

        cameraToRotate.SetActive(isTurretActive);
        player.SetActive(!isTurretActive);

        Debug.LogError($"Турель активна: {StaticHolder.isTurretActive}");
    }

    private void HandleTurretControl()
    {
        CheckFire();
        RotateCamera();
        AdjustSensitivity();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleTurret();
            isPlayerHere = true;
        }
    }

    private void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        rotationX = Mathf.Clamp(rotationX - mouseY, -90f, 90f);
        rotationY += mouseX;

        cameraToRotate.transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerHere = true;
            Debug.Log($"Игрок здесь: {isPlayerHere}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerHere = false;
        }
    }

    private void AdjustSensitivity()
    {
        if (Input.GetKey(KeyCode.UpArrow))
            mouseSensitivity = Mathf.Clamp(mouseSensitivity + Time.deltaTime * 40, 50, 300);

        if (Input.GetKey(KeyCode.DownArrow))
            mouseSensitivity = Mathf.Clamp(mouseSensitivity - Time.deltaTime * 40, 50, 300);
    }

    private void CheckFire()
    {
        if (Input.GetButton("Fire1") && currentTemperature < maxTemperature)
        {
            laser.SetActive(true);
            if (laser.transform.localScale.z < 200)
                laser.transform.localScale += new Vector3(0, 0, 0.8f);
            laser.transform.Rotate(0, 0, 1000 * Time.deltaTime);
            if (currentTemperature <= maxTemperature)
                currentTemperature += Time.deltaTime;
        }
        else
        {
            laser.SetActive(false);
            laser.transform.localScale = new Vector3(0.23f, 0.23f, 1);
            laser.transform.Rotate(0, 0, 0);

            UpdateTemperature(-Time.deltaTime);
        }
    }

    private void UpdateTemperature(float value)
    {
        currentTemperature = Mathf.Clamp(currentTemperature + value, 0, maxTemperature);
    }
}
