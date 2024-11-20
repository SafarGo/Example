using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReservoirController : MonoBehaviour
{
    [SerializeField] int maxHoneyCount;
    int currentHuneyCount;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Honey"))
        {
            if (currentHuneyCount < maxHoneyCount)
            {
                currentHuneyCount++;
            }
            Destroy(collision.gameObject);
            Debug.Log(currentHuneyCount);
        }
    }
}
