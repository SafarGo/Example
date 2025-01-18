using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class SmokeController : MonoBehaviour
{
    bool isSlep;
    bool isBig = false;
    float t;
    GameObject capsule;
    private void Start()
    {
        capsule = gameObject.transform.Find("Sphere--DNS--").gameObject;
        Destroy(gameObject, 180);
    }
    private void OnCollisionEnter(Collision other)
    {
        Destroy(capsule);
        if (other.gameObject.tag != "Car")
        {
            Destroy(gameObject.GetComponent<Collider>());
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            isSlep = true;
        }
    }
    private void FixedUpdate()
    {
        if (isSlep)
        {
            t += Time.deltaTime;
            if (t <= 120)
            {
                if (transform.localScale.x <= 16f)
                    transform.localScale += new Vector3(0.5f * Time.deltaTime, 0.5f * Time.deltaTime, 0.5f * Time.deltaTime);
            }
            else
            {
                if (transform.localScale.x >= 0.01f)
                    transform.localScale -= new Vector3(2 * Time.deltaTime, 2 * Time.deltaTime, 2 * Time.deltaTime);
                else
                    Destroy(gameObject);
            }
        }
    }



}
