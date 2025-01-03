using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PauseMenu : MonoBehaviour
{
    public GameObject panel;
    public GameObject BestiariyPanel;
    bool isStopped;
    bool isBestiariyOpened = false;
    private FirstPersonController firstPersonController;

    private void Start()
    {
        firstPersonController = GameObject.Find("Player").GetComponent<FirstPersonController>();
        BestiariyPanel.SetActive(false);
    }

    private void Update()
    {
        //if (!StaticHolder.isTurretActive)
        //{
            if (Input.GetKeyDown(KeyCode.Escape) && !isStopped)
            {
                Stop();

            }
            else if (Input.GetKeyDown(KeyCode.Escape) && isStopped)
            {
                Con();
            }
            if (Input.GetKeyDown(KeyCode.V))
            {
                if (StaticHolder.isCanOpenUI)
                {
                    BestiariyMenu();
                }
            }
        //}
    }

    public void Stop()
    {
        //Time.timeScale = 0.0f;
        panel.SetActive(true);
        isStopped = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        firstPersonController.cameraCanMove = false;
        StaticController.instance.TurnCursorInState(false);
    }
    public void Con()
    {
        //Time.timeScale = 1.0f;
        panel.SetActive(false);
        isStopped = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.None;
        firstPersonController.cameraCanMove = true;
        StaticController.instance.TurnCursorInState(true);
    }

    void BestiariyMenu()
    {
        isBestiariyOpened = !isBestiariyOpened;
        Con();
        panel.SetActive(isStopped);
        if (StaticHolder.isCanOpenUI)
        {
            StaticController.instance.TurnCursorInState(!isBestiariyOpened);
            BestiariyPanel.SetActive(isBestiariyOpened);
        }
    }
}
