using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RamWaspController : MainWaspController
{
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
       
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 6)
        {
            RamObstacle(collision.gameObject);
        }
    }

    void RamObstacle(GameObject gameobj)
    {
        Destroy(gameobj);
        WaspDeath();
    }
}
