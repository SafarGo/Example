using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceWaspController : MainWaspController
{
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private int spread;
    [SerializeField]
    private Transform spawnPoint;
    [SerializeField]
    private float spawnSpeed = 10;
    private float t = 0f;

    protected override void FixedUpdate()
    {
        spawnPoint.LookAt(TowardObstacle);
        base.FixedUpdate();
        if (distance >= 50)
            agent.SetDestination(TowardObstacle.position);
        else
        {
            agent.SetDestination(gameObject.transform.position);
            t += Time.deltaTime;
            if (t >= 2)
            {
                
                Attack();
                t = 0f;
            }
        }
    }

    void Attack()
    {
        if (bulletPrefab != null && TowardObstacle != null)
        {
            // Создаём объект
            GameObject spawnedObject = Instantiate(bulletPrefab, spawnPoint.position, Quaternion.identity);

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
}
