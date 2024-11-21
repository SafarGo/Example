using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectOnGridSpawner : MonoBehaviour
{
    [SerializeField] GameObject GitrdSpawnObjectPrefab;
    [SerializeField] Transform SpawnPoint;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Instantiate(GitrdSpawnObjectPrefab, new Vector3(Mathf.CeilToInt(SpawnPoint.position.x) / 2 * 2,0, Mathf.CeilToInt(SpawnPoint.position.z) / 2 * 2), Quaternion.identity);
            Debug.Log("Spawn");
        }
    }
}
