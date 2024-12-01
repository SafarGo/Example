using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject panel;
    bool isStopped;
    private FirstPersonController firstPersonController;

    private void Start()
    {
        firstPersonController = GameObject.Find("Player").GetComponent<FirstPersonController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M) && !isStopped)
        {
            Stop();
            
        }
        else if(Input.GetKeyDown(KeyCode.M) && isStopped)
        {
            Con();
        }
    }

    public void Stop()
    {
        //Time.timeScale = 0.0f;
        panel.SetActive(true);
        isStopped = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        firstPersonController.cameraCanMove = false;
    }
    public void Con()
    {
        //Time.timeScale = 1.0f;
        panel.SetActive(false);
        isStopped = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.None;
        firstPersonController.cameraCanMove = true;
    }
}
