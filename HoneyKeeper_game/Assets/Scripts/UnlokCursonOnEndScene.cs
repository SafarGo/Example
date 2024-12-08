using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlokCursonOnEndScene : MonoBehaviour
{


    // Update is called once per frame
    void FixedUpdate()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
