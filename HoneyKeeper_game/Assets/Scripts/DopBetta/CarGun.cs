using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
    public class Features
    {
        public string name;
        public GameObject Bullet;
        public int bulletSpeed;
        public int spread;
        public float fireRate;
        public Color sphereColor;
    }
public class CarGun : MonoBehaviour
{



    public List<Features> features = new List<Features>();

    [Header("Настройки выпуска")]
    public GameObject objectToSpawn; // Объект, который будет выпускаться
    public Transform spawnPoint; // Точка выпуска
    public float spawnSpeed = 10f; // Скорость выпускаемых объектов
    public float spread = 15f; // Угол разброса в градусах
    public float spawnInterval = 0.2f; // Интервал между выпусками при удержании кнопки
    private int watBulletNow = 0;
    private bool isShooting = false; // Флаг удержания кнопки
    [Header("Настройки визуала оружия")]
    [SerializeField] Renderer sphereColor;

    public Animator anim;

    private void Start()
    {
        StaticHolder.isCanFire = true;
        SetBullets(0);
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
        if(Input.GetKeyDown(KeyCode.Q))
        {
            if (watBulletNow == 0)
            {
                watBulletNow++;
            }
            else
            {
                watBulletNow = 0;
            }
            SetBullets(watBulletNow);
        }
    }

    private void SpawnObject()
    {
        if (objectToSpawn != null && spawnPoint != null && StaticHolder.isCanFire)
        {
            // Создаём объект
            GameObject spawnedObject = Instantiate(objectToSpawn, spawnPoint.position, Quaternion.identity);

            // Применяем разброс
            Vector3 randomDirection = Quaternion.Euler(
                UnityEngine.Random.Range(-spread, spread),
                UnityEngine.Random.Range(-spread, spread),
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

    public void SetBullets(int bulletID)
    {
        objectToSpawn = features[bulletID].Bullet;
        spawnSpeed = features[bulletID].bulletSpeed;
        spread = features[bulletID].spread;
        spawnInterval = features[bulletID].fireRate;
        sphereColor.material.color = features[bulletID].sphereColor;
    }
}
