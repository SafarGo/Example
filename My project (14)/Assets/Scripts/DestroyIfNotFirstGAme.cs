using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyIfNotFirstGAme : MonoBehaviour
{
    private void Start()
    {
        JsonSaver._instance.Load();
        if(!StaticHolder.isFirstGame)
        {
            Destroy(gameObject);
        }
    }
}
