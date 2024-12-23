using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeDestoyer : MonoBehaviour
{
    [SerializeField]float i;
    void Update()
    {
        Destroy(gameObject, i);
    }
}
