using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class RamWaspController : MainWaspController
{
    bool isAtaking;
    GameObject target;
    Rigidbody rb;
    protected override void Start()
    {
        base.Start();
        target = TowardObstacle.gameObject;
        rb = gameObject.GetComponent<Rigidbody>();
        if(target == null)
        {
            Start();
        }
    }
    protected override void FixedUpdate()
    {
        if (target != null)
        {
            base.FixedUpdate();
            if (distance <= 15)
            {
                Destroy(agent);

                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, 10f * Time.deltaTime);
                rb.isKinematic = false;
                //RamObstacle(target);
            }
        }
        else
        {
            base.Start();
        }
        

    }

    private void OnCollisionEnter(Collision collision)
    {
        RamObstacle(target.gameObject);
    }


    void RamObstacle(GameObject gameobj)
    {
        Destroy(gameobj);
        WaspDeath();
    }
}
