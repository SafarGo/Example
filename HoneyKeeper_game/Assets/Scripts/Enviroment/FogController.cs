using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogController : MonoBehaviour
{
    [SerializeField] Color _fogColor;

    private void Start()
    {
        RenderSettings.fogColor = _fogColor;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(gameObject.CompareTag("Player"))
        {
            RenderSettings.fogColor = _fogColor;
        }
    }


}
