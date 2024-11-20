using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    [SerializeField] public static EventManager Instance { get; private set; }
    private void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }


    public void Cleaned()
    {
        Debug.Log("Cleaned");
    }

    public void Fixed()
    {
        Debug.Log("Fixed");
    }    
}
