using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DestoryingStoneByWaspsStoneController : MonoBehaviour
{
    float t;
     Material hz;
    //AudioSource audioRastvr;
    void Start()
    {
        //audioRastvr = gameObject.GetComponent<AudioSource>();
        //audioRastvr.mute = true;
        t = -7;
        hz = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (StaticHolder.isWaspsMadeFirstAttak == true)
        {
            if (t >= 0.7f)
            {
                BarierDestroyingCatScene.instance.OffKat_1();
                Destroy(gameObject);
            }
            if(t >= -0.35)
            {
                //audioRastvr.Play();
            }
            //t += Time.deltaTime;
            if (t < -0.44f)
            {
                t += Time.deltaTime;
                BarierDestroyingCatScene.instance.OnKat_1();
            }
            else
            {
                t += Time.deltaTime / 15;
            }
            hz.SetVector("_DissolveOffest", new Vector3(0, t, 0));
        }
    }
}
