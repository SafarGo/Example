using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarageController : MonoBehaviour
{
    [SerializeField] GameObject CameraToShow;
    [SerializeField] GameObject CameraToHide;
    [SerializeField] CarMovementController Car;
    [SerializeField] GameObject ToolTip;
    bool isIn;

    //private void Start()
    //{
    //    StaticHolder.isTurretActive = false;
    //}
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            isIn = true;
            ToolTip.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isIn = false;
            ToolTip.SetActive(false);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && isIn)
        {
            StaticHolder.isCanFire = true;
            StaticHolder.isTurretActive = true;
            CameraToShow.SetActive(true);
            CameraToHide.SetActive(false);
            Car.isCanMove = true;
            ToolTip.SetActive(false);
        }
    }
}
