using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class RamWaspController : MainWaspController
{
    protected override void FixedUpdate()
    {

        base.FixedUpdate();
        GameObject target = TowardObstacle.gameObject;
        if (distance <= 15)
        {
            Destroy(agent);
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, 10f * Time.deltaTime);
            RamObstacle(target);
        }

    }

   

    void RamObstacle(GameObject gameobj)
    {
        Destroy(gameobj);
        WaspDeath();
    }
}
