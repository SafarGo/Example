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
    int count;


    protected override void Start()
    {
        base.Start();
        if(TowardObstacle.gameObject.GetComponent<ObjectPlacer>().ObjectType != "Conveyer" && count < 3)
        {
            count++;
            Start();
        }
    }
    protected override void FixedUpdate()
    {
        spawnPoint.LookAt(TowardObstacle.position);
        base.FixedUpdate();

        if (distance <= 10)
        {
            //agent.SetDestination(gameObject.transform.position);
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
