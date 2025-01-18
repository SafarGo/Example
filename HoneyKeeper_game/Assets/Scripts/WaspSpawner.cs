using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaspSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> waspTypes; // Список типов ос
    [SerializeField] private int spawnCount = 5; // Количество ос для спавна
    [SerializeField] private float ramWaspChance = 0.5f; // Шанс спавна осы-тарана
    [SerializeField] private float fireWaspChance = 1f; // Шанс спавна огненной осы
    [SerializeField] private Vector2 spawnDelayRange = new Vector2(2f, 5f); // Диапазон задержки между спавнами
    [SerializeField] private Vector2 spawnMomentRange = new Vector2(200f, 340f); // Диапазон времени до следующей волны
    [SerializeField] private Vector3 spawnAreaSize = new Vector3(6f, 0f, 6f); // Размер области спавна

    private float timeToSpawn; // Время до следующего спавна
    private float timeToFirstAttack; // Время до первой атаки
    private bool isAttacking; // Флаг атаки
    private bool isFirstLaunch; // Флаг первого запуска

    private void Start()
    {
        if (StaticHolder.count_of_simple_honey >= 100 && StaticHolder.count_of_enegry_honey >= 75)
        {
            isFirstLaunch = true;
        }
        SetNextSpawnMoment();
    }

    private void FixedUpdate()
    {
        if (!isAttacking)
        {
            timeToSpawn += Time.deltaTime;
            if (timeToSpawn >= spawnMomentRange.x && isFirstLaunch)
            {
                isAttacking = true;
                StartCoroutine(SpawnWasps());
            }
        }

         if (!StaticHolder.isWaspsMadeFirstAttak)
         {
             timeToFirstAttack += Time.deltaTime;
             if (timeToFirstAttack >= 480)
             {
                 isFirstLaunch = true;
                 StaticHolder.isWaspsMadeFirstAttak = true;
                 StartCoroutine(SpawnWasps());
             }
         }
    }

    private IEnumerator SpawnWasps()
    {
        timeToSpawn = 0;

        for (int i = 0; i < spawnCount; i++)
        {
            GameObject waspToSpawn = SelectWaspType();
            Vector3 spawnPosition = transform.position + new Vector3(
                Random.Range(-spawnAreaSize.x, spawnAreaSize.x),
                spawnAreaSize.y,
                Random.Range(0, spawnAreaSize.z)
            );

            Instantiate(waspToSpawn, spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(spawnDelayRange.x, spawnDelayRange.y));
        }

        SetNextSpawnMoment();
        isAttacking = false;
    }

    private GameObject SelectWaspType()
    {
        float rnd = Random.value;
        if (rnd <= ramWaspChance)
            return waspTypes[0];
        if (rnd <= fireWaspChance)
            return waspTypes[1];
        return waspTypes[2];
    }

    private void SetNextSpawnMoment()
    {
        timeToSpawn = 0;
        spawnMomentRange.x = Random.Range(spawnMomentRange.x, spawnMomentRange.y);
    }
}
