using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaspSpawner : MonoBehaviour
{
    [SerializeField] GameObject WaspPrefab;
    [SerializeField]float timeToSpawn;
    [SerializeField]int spawnMoment;
    [SerializeField] int spawnCount = 5;
    [SerializeField] bool isAttaking;
    //bool isHaveHoney;
    bool isFirstZapusk;
    private void Start()
    {
        if(StaticHolder.count_of_simple_honey >= 100 && StaticHolder.count_of_enegry_honey >= 75)
            isFirstZapusk = true;
        spawnMoment = Random.Range(120, 240);
    }
    private void FixedUpdate()
    {
        if (!isAttaking)
        {
            timeToSpawn += Time.deltaTime;
            if (timeToSpawn >= spawnMoment && isFirstZapusk == true)
            {
                timeToSpawn = 0;
                isAttaking = true;
                StartCoroutine(SpwnWaspCorutine());
            }
        }
        if(StaticHolder.count_of_simple_honey >= 100 && StaticHolder.count_of_enegry_honey >= 75 && !isFirstZapusk)
        {
            isFirstZapusk =  true;
            StartCoroutine(SpwnWaspCorutine());
        }
    }
    // Update is called once per frame

    IEnumerator SpwnWaspCorutine()
    {
        timeToSpawn = 0;
        for (int i = 0; i < spawnCount; i++)
        {
            Instantiate(WaspPrefab,transform.position + new Vector3(Random.Range(-6,6),0, Random.Range(0, 6)),Quaternion.identity);
            yield return new WaitForSeconds(3f);
        }
        spawnMoment = Random.Range(5, 10);
        isAttaking = false;
    }
}
