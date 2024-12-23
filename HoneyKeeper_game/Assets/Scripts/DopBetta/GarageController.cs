using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarageController : MonoBehaviour
{
    [SerializeField] GameObject CameraToShow;
    [SerializeField] GameObject CameraToHide;
    [SerializeField] CarMovementController Car;
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            CameraToShow.SetActive(true);
            CameraToHide.SetActive(false);
            Car.isCanMove = true;
        }
    }
}
