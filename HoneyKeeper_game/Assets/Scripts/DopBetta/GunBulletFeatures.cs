using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GunBulletFeatures : MonoBehaviour
{
    protected List<Features> features;
    protected class Features
    {
        public GameObject Bullet;
        public int spread;
        public int bulletSpeed;
        public int fireRate;
    }
    void SetFeature()
    {

    }
}
