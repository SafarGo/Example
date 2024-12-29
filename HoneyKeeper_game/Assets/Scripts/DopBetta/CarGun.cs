using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarGun : MonoBehaviour
{
    [Header("Настройки выпуска")]
    public GameObject objectToSpawn; // Объект, который будет выпускаться
    public Transform spawnPoint; // Точка выпуска
    public float spawnSpeed = 10f; // Скорость выпускаемых объектов
    public float spread = 15f; // Угол разброса в градусах
    public float spawnInterval = 0.2f; // Интервал между выпусками при удержании кнопки

    private bool isShooting = false; // Флаг удержания кнопки

    public Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Проверка нажатия кнопки
        if (Input.GetMouseButtonDown(0)) // ЛКМ нажата
        {
            if (anim != null) { anim.SetBool("isFire", true); }
            SpawnObject(); // Выпустить один объект
            StartCoroutine(StartShooting());
        }

        if (Input.GetMouseButtonUp(0)) // ЛКМ отпущена
        {
            if (anim != null) { anim.SetBool("isFire", false); }
            StopShooting();
        }
    }

    private void SpawnObject()
    {
        if (objectToSpawn != null && spawnPoint != null && StaticHolder.isTurretActive)
        {
            // Создаём объект
            GameObject spawnedObject = Instantiate(objectToSpawn, spawnPoint.position, Quaternion.identity);

            // Применяем разброс
            Vector3 randomDirection = Quaternion.Euler(
                Random.Range(-spread, spread),
                Random.Range(-spread, spread),
                0
            ) * spawnPoint.forward;

            // Задаём скорость объекту
            Rigidbody rb = spawnedObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = randomDirection.normalized * spawnSpeed;
            }
        }
    }

    private IEnumerator StartShooting()
    {
        isShooting = true;
        while (isShooting)
        {
            yield return new WaitForSeconds(spawnInterval); // Ждать перед следующим выпуском
            SpawnObject();
        }
    }

    private void StopShooting()
    {
        isShooting = false; // Остановить выпуск
    }
}
