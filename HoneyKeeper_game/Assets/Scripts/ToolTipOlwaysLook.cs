using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTipOlwaysLook : MonoBehaviour
{
    Camera mainCamera;
    private void Start()
    {
        mainCamera = Camera.main;
    }
    void Update()
    {
        gameObject.transform.LookAt(mainCamera.transform);
    }
}
