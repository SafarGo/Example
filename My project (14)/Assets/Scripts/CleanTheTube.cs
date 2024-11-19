using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class CleanTheTube : MonoBehaviour
{
    public GameObject canvas;
    public TMP_Text text;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            canvas.SetActive(true);
            text.text = "Press E";
        }
    }
    private void OnTriggerStay(Collider other)
    {
        
        if (other.gameObject.tag == "Player")
        {
            
            if (Input.GetKey(KeyCode.E))
            {
                StartCoroutine(CountDown());
            }
        }
    }

    IEnumerator CountDown()
    {
        int t = 3;
        while(t >= 0)
        {
            text.text = t.ToString() + "...";
            yield return new WaitForSeconds(1);
            t--;
        }
        text.text = "";
    }

    private void OnTriggerExit(Collider other)
    {
        canvas.SetActive(false);
    }
}
