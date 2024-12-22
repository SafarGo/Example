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
    private void Start()
    {
        spawnMoment = Random.Range(120, 240);
    }
    private void FixedUpdate()
    {
        if (!isAttaking)
        {
            timeToSpawn += Time.deltaTime;
            if (timeToSpawn >= spawnMoment)
            {
                timeToSpawn = 0;
                isAttaking = true;
                StartCoroutine(SpwnWaspCorutine());
            }
        }
    }
    // Update is called once per frame

    IEnumerator SpwnWaspCorutine()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Instantiate(WaspPrefab,transform.position + new Vector3(Random.Range(-6,6),0, Random.Range(0, 6)),Quaternion.identity);
            yield return new WaitForSeconds(3f);
        }
        spawnMoment = Random.Range(5, 10);
        isAttaking = false;
    }
}
