using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FireDamager : MonoBehaviour
{
    public WaspType waspType;
    bool osa;

    void Start()
    {
        Destroy(gameObject, 15);
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.TryGetComponent(out MainWaspController waspController))
        {
            waspController.WaspDeath();
            return;
        }
        if(other.TryGetComponent(out FireWaspController fireWaspController))
        {
            fireWaspController.WaspDeath();
            return;
        }
    }
}

public enum WaspType
{
    fireWasp,
    frostWasp,
    DefaultWasp,
    TaranWasp
}
