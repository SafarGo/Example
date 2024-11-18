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
    [SerializeField] int honeyCreateRate;
    int t;


    void HoneyCreating()
    {
        for (int i = 0; i < cartridgeCount; i++)
        {
        Instantiate(honeyBottlePrefab, spawnPoint.position,Quaternion.identity);
        }
    }
    private void FixedUpdate()
    {
        if(t > honeyCreateRate)
        {
            t = 0;
            HoneyCreating();
        }
    }
}
