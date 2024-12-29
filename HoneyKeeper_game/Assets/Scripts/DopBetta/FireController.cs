using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour
{
    bool isSlep;
    bool isBig = false;

    private void Start()
    {
        Destroy(gameObject,25);
    }
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
            if (!isBig)
            {
                if (transform.localScale.x <= 3f)
                    transform.localScale += new Vector3(0.2f * Time.deltaTime, 0.2f * Time.deltaTime, 0.2f * Time.deltaTime);
                else
                    isBig = true;
            }
            else
            {
                if (transform.localScale.x >= 0.1f)
                    transform.localScale -= new Vector3(0.15f * Time.deltaTime, 0.15f * Time.deltaTime, 0.15f * Time.deltaTime);
                else
                    Destroy(gameObject);
            }
        }
    }


}

