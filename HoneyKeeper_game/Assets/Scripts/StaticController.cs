using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticController : MonoBehaviour
{
    public static StaticController instance;
    // Start is called before the first frame update
    [SerializeField] GameObject centerCursor;
    [SerializeField] GameObject centerCursor2;
    float constantMoveingSpeed;
    void Start()
    {
        if (instance == null)
            instance = this;
        centerCursor = GameObject.Find("Center--DNS--");
        centerCursor2 = GameObject.Find("CrosshairAndStamina");
    }

    public void TurnCursorInState(bool state)
    {
        centerCursor.SetActive(state);
        centerCursor2.SetActive(state);
    }

    public void SetMoveingSpeed(GameObject objecttoChandgespeed,int speed, int sprintspeed)
    {
        objecttoChandgespeed.GetComponent<FirstPersonController>().walkSpeed = speed;
        objecttoChandgespeed.GetComponent<FirstPersonController>().sprintSpeed = sprintspeed;
    }

}
