using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    protected int damage;
    protected int speed;

    public void Initizaleze(int dam, int sp)
    {
        damage = dam;
        speed = sp;
    }

    public void Spawn(Vector3 spawnpoint)
    {
        Vector2 rp = Random.insideUnitCircle * 3;
        transform.position = spawnpoint + new Vector3(rp.x, 0, rp.y);
    }

    public abstract void Cry();
}
