using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarageController : MonoBehaviour
{
    [SerializeField] GameObject CameraToShow;
    [SerializeField] GameObject CameraToHide;
    [SerializeField] CarMovementController Car;
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
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isIn = false;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && isIn)
        {
            StaticHolder.isTurretActive = true;
            CameraToShow.SetActive(true);
            CameraToHide.SetActive(false);
            Car.isCanMove = true;
        }
    }
}
