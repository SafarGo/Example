using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiveController : MonoBehaviour
{
    [SerializeField] GameObject honeyBottlePrefab;
    [SerializeField] Transform spawnPoint;
    [SerializeField] int cartridgeCount;
    [SerializeField] int beesCount;
    [SerializeField] int currentHuneyCount;
    [SerializeField] int honeyCreateRate = 120;
    float t;

    private void Start()
    {
        Time.timeScale = 1;
        //honeyCreateRate = 12;// Должно быть 120
    }
    private IEnumerator HoneyCreatingCoroutine()
    {
        for (int i = 0; i < cartridgeCount; i++)
        {
            Instantiate(honeyBottlePrefab, spawnPoint.position, Quaternion.identity);
            // Задержка между созданиями
            yield return new WaitForSeconds(0.5f); // Здесь указываем нужную задержку в секундах
        }
    }
    private void FixedUpdate()
    {
        t += Time.deltaTime;
        if (t > honeyCreateRate)
        {
            t = 0;
            StartCoroutine(HoneyCreatingCoroutine());
        }
        //Debug.Log(t);
    }

    void UpdateBeesCount(int plusBees)
    {
        if (beesCount % 2 == 0)
        {
            honeyCreateRate = 120 / beesCount;
        }

    }
}
