using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class FireWaspController : MainWaspController
{
    [SerializeField] Transform spawnPoint;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] int spread;
    [SerializeField] int spawnSpeed;
    private float t = 0f;
    private Transform PointToFire;
    protected override void Start()
    {
        base.Start();
        PointToFire = TowardObstacle.gameObject.transform.Find(nameof(PointToFire));
        if(PointToFire == null)
        {
            Start();
        }
    }
  protected override void FixedUpdate()
  {
        spawnPoint.LookAt(PointToFire);
        base.FixedUpdate();
        if (distance >= 50)
            agent.SetDestination(TowardObstacle.position);
        else
        {
            agent.SetDestination(gameObject.transform.position);
            t += Time.deltaTime;
            if (t >= 2)
            {

                FireToObstacle();
                t = 0f;
            }
        }
  }

    void FireToObstacle()
    {
        if (bulletPrefab != null && TowardObstacle != null)
        {
            // Создаём объект
            GameObject spawnedObject = Instantiate(bulletPrefab, spawnPoint.position, Quaternion.identity);
            spawnedObject.transform.Find("FireDamager").GetComponent<FireDamager>().ParentObject = gameObject;
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
