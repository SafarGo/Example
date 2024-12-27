using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour
{
    bool isSlep;
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag != "Car")
        {
            Destroy(gameObject.GetComponent<Collider>());
            Destroy(gameObject.GetComponent<Rigidbody>());
            isSlep = true;
        }
    }
    private void FixedUpdate()
    {
        if(isSlep)
        {
            if (transform.localScale.x >= 0.1f)
                transform.localScale -= new Vector3(0.1f * Time.deltaTime, 0.1f * Time.deltaTime, 0.1f * Time.deltaTime);
            else
                Destroy(gameObject);
        }
    }


}

