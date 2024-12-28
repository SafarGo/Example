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
    private void Start()
    {
        target = TowardObstacle.gameObject;
    }
    protected override void FixedUpdate()
    {

        base.FixedUpdate();
        if (distance <= 15)
        {
                Destroy(agent);

            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, 10f * Time.deltaTime);
            //RamObstacle(target);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        RamObstacle(TowardObstacle.gameObject);
    }


    void RamObstacle(GameObject gameobj)
    {
        Destroy(gameobj);
        WaspDeath();
    }
}
