using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class FireWaspController : MainWaspController
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int spread;
    [SerializeField] private int spawnSpeed;

    private float fireCooldown;
    private Transform pointToFire;

    protected override void Start()
    {
        base.Start();
        pointToFire = TowardObstacle?.Find(nameof(pointToFire));

        // Если точка не найдена, избегаем рекурсии, используя альтернативный способ.
        if (pointToFire == null)
        {
            Debug.LogWarning("PointToFire not found. Setting default target.");
            pointToFire = TowardObstacle; // Устанавливаем основной объект как цель.
        }
    }

    protected override void FixedUpdate()
    {
        if (pointToFire != null && spawnPoint != null)
        {
            spawnPoint.LookAt(pointToFire); // Спавн-точка всегда смотрит на цель.
        }

        base.FixedUpdate();

        if (distance >= 50)
        {
            agent.SetDestination(TowardObstacle.position);
        }
        else
        {
            agent.SetDestination(transform.position); // Останавливаемся перед атакой.
            HandleFiring();
        }
    }

    private void HandleFiring()
    {
        fireCooldown += Time.deltaTime;
        if (fireCooldown >= 2f)
        {
            FireToObstacle();
            fireCooldown = 0f;
        }
    }

    private void FireToObstacle()
    {
        if (bulletPrefab == null || TowardObstacle == null || spawnPoint == null) return;

        // Создаём пулю.
        GameObject spawnedBullet = Instantiate(bulletPrefab, spawnPoint.position, Quaternion.identity);

        // Настраиваем родителя.
        var fireDamager = spawnedBullet.transform.Find("FireDamager")?.GetComponent<FireDamager>();
        if (fireDamager != null)
        {
            fireDamager.ParentObject = gameObject;
        }

        // Рассчитываем направление с учётом разброса.
        Vector3 randomDirection = Quaternion.Euler(
            Random.Range(-spread, spread),
            Random.Range(-spread, spread),
            0
        ) * spawnPoint.forward;

        // Применяем скорость пули.
        Rigidbody rb = spawnedBullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = randomDirection.normalized * spawnSpeed;
        }
    }
}
