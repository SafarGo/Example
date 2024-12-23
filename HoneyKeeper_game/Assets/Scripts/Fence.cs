using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fence : MonoBehaviour
{
    [SerializeField]
    private GameObject f;
    [SerializeField]
    private float scaleX, scaleY, scaleZ;
    [SerializeField]
    private int countToSpawn;
    [SerializeField]
    private float distX, distZ;

    private void Start()
    {
        Invoke("Spawn",0);
    }

    public void Spawn()
    {
        for (int i = 0; i < countToSpawn; i++)
        {
            GameObject sp = Instantiate(f, transform.position + new Vector3(i*5, 0, 0), transform.rotation);
            sp.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
            GameObject sp1 = Instantiate(f, transform.position + new Vector3(i * 5, 0, distZ), transform.rotation);
            sp1.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
        }
        for (int i = 0; i < countToSpawn; i++)
        {
            GameObject sp = Instantiate(f, transform.position + new Vector3(0, 0, i*5), transform.rotation);
            sp.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
            sp.transform.Rotate(0,0,0);
            GameObject sp1 = Instantiate(f, transform.position + new Vector3(distX, 0, i*5), transform.rotation);
            sp1.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
            sp.transform.Rotate(0, 0, 0);
        }
    }
}
