using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOpen : MonoBehaviour
{
    private void FixedUpdate()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
