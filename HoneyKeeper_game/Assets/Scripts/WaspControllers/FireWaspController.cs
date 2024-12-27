using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class FireWaspController : MainWaspController
{

    void Satrt()
    {

    }
  protected override void FixedUpdate()
  {
      base.FixedUpdate();
        if (distance >= 100)
            agent.SetDestination(TowardObstacle.position);
        else
        {
            agent.SetDestination(gameObject.transform.position);
        }
        FireToObstacle();
  }

    void FireToObstacle()
    {

    }
}
