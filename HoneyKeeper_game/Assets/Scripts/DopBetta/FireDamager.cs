using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FireDamager : MonoBehaviour
{
    public WaspType waspType;
    public GameObject ParentObject;
    bool osa;

    void Start()
    {
        Destroy(gameObject, 15);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != ParentObject)
        {
            if (other.TryGetComponent(out MainWaspController waspController))
            {
                waspController.WaspDeath();
                return;
            }
            if (other.TryGetComponent(out FireWaspController fireWaspController))
            {
                fireWaspController.WaspDeath();
                return;
            }

            if (other.TryGetComponent(out RamWaspController ramWaspController))
            {
                ramWaspController.WaspDeath();
                Destroy(ramWaspController);
                return;
            }
            if (other.TryGetComponent(out HiveController hiveController))
            {
                hiveController.UpdateHP(-2);
                return;
            }
            if (other.TryGetComponent(out FlowersFresh ClumbController))
            {
                ClumbController.ForceDrying(-15);
                return;
            }
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
