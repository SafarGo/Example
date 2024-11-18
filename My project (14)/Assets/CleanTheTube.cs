using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanTheTube : MonoBehaviour
{
    public GameObject canvas;
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            canvas.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        canvas.SetActive(false);
    }
}
