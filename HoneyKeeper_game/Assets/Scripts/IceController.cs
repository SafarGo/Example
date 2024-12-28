using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceController : MonoBehaviour
{
    bool isInFire;
    float destroyingSpeed = 0.1f;
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag != "Car")
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }


    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.tag != "Car")
    //    {
    //        gameObject.GetComponent<Rigidbody>().isKinematic = true;
    //    }
    //}

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Fire")
        {
            destroyingSpeed = 3;
        }
        else
        {
            destroyingSpeed = 0.1f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        destroyingSpeed = 0.1f;
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
    }

    private void FixedUpdate()
    {
        //if (isInFire)
        //{
        if (transform.localScale.x >= 0.1)
                transform.localScale -= new Vector3(destroyingSpeed * Time.deltaTime, destroyingSpeed * Time.deltaTime, destroyingSpeed * Time.deltaTime);
            else
                Destroy(gameObject);
        Debug.Log("destroyingSpeed " + destroyingSpeed);
        //}
    }
}
