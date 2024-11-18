using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CleanTheTube : MonoBehaviour
{
    public GameObject canvas;
    public TMP_Text textMeshPro;
    private EventManager eventManager;

    private void Start()
    {
        eventManager = new EventManager();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            canvas.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        canvas.SetActive(false);
    }


    private void OnTriggerStay(Collider other)
    {
        
        if (other.gameObject.tag == "Player")
        {
            canvas.SetActive(true);
            

            if (Input.GetKey(KeyCode.E))
            {
                StartCoroutine(CountDown());
                
            }
        }
    }

    private IEnumerator CountDown()
    {
        int t = 3;
        textMeshPro.text = t.ToString();
        while(t>=0)
        {
            textMeshPro.text = t.ToString() + "...";
            yield return new WaitForSeconds(1);
            t--;
        }
        textMeshPro.text = "Press E";
        eventManager.Cleaned();
    }
}
