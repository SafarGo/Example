using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewController : MonoBehaviour
{
    [SerializeField] GameObject bear;
    [SerializeField] List <GameObject> Cameras = new List<GameObject>();
    bool FirstView = true;
    void ChandgeCamera(int CameraIndex)
    {
        for (int i = 0; i < Cameras.Count; i++)
        {
            if(i != CameraIndex)
            {
                Cameras[i].SetActive(false);
            }
            else
            {
                Cameras[CameraIndex].SetActive(true);
            }
        }
    }

    void ChandgeMainView()
    {
        FirstView = !FirstView;
        if(FirstView == true)
        {
            Cameras[0].SetActive(true);
            Cameras[1].SetActive(false);
            bear.SetActive(true);
           Cursor.lockState = CursorLockMode.Locked;
           Cursor.visible = false;
        }
        else
        {
            Cameras[0].SetActive(false);
            Cameras[1].SetActive(true);
            bear.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        Debug.Log(FirstView);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            ChandgeMainView();
        }
    }
}
