using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaspSpawner : MonoBehaviour
{
    [SerializeField] List <GameObject> WaspsTypes;
    [SerializeField]float timeToSpawn;
    [SerializeField]int spawnMoment;
    [SerializeField] int spawnCount = 5;
    [SerializeField] bool isAttaking;
    [Header("Вероятность спавна осы - тарана")]
    [Range(0f, 0.5f)]
    public float RamWaspWar;
    [Range(0f, 1f)]
    public float FireWaspSapwn;

    //bool isHaveHoney;
    bool isFirstZapusk = false;
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
        GameObject _currentWasp = null;
        timeToSpawn = 0;
        for (int i = 0; i < spawnCount; i++)
        {
            float rnd = Random.Range(0, 2);
            if (rnd <= RamWaspWar)
            {
                _currentWasp = WaspsTypes[0];
            }
            if (rnd < RamWaspWar && rnd <= FireWaspSapwn)
            {
                _currentWasp = WaspsTypes[1];
            }
            if (rnd > FireWaspSapwn)
            {
                _currentWasp = WaspsTypes[2];
            }
            Instantiate(_currentWasp, transform.position + new Vector3(Random.Range(-6,6),0, Random.Range(0, 6)),Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(2,5));
        }
        spawnMoment = Random.Range(200, 340);
        isAttaking = false;
    }
}
