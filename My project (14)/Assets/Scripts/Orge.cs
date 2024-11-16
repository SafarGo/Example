using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orge : Unit
{
    private void Start()
    {
        damage = 2;
        speed = 2;
    }


    public override void Cry()
    {
        Debug.Log("Ogre");
    }
}
