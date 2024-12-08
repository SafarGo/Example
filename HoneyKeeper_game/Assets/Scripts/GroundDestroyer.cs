using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDestroyer : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Honey") || collision.gameObject.CompareTag("EnergoHoney"))
        {
            Destroy(collision.gameObject);
        }
    }
}
