using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavedObjectsSpawnerController : MonoBehaviour
{
    // Start is called before the first frame update\
    private void Awake()
    {
        JsonSaver._instance.Load();
    }
    void Start()
    {

        TestSpawn();
    }

    void TestSpawn()
    {
        for (int i = 0; i < StaticHolder.AllSpawnedObjects.Count; i++)
        {
            Instantiate(StaticHolder.AllSpawnedObjects[i]);
        }
        Debug.Log("LLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL");
    }

}
