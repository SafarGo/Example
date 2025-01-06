using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectsPicker : MonoBehaviour
{
    [SerializeField] string pickObjectName;
    [SerializeField] GameObject ObjectToPick;
    [SerializeField] Sprite pickObjectIcon;

    bool isPlayerOn;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            isPlayerOn = true;
        }
        else
        {
            isPlayerOn = false;
        }
        //Debug.LogError(isPlayerOn);
    }

    private void Update()
    {
        //Debug.LogError(isPlayerOn);
        if(isPlayerOn == true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                for (int i = 0; i < ObjectInHandController.instance.objectsInHand.Count; i++)
                {
                    if (ObjectInHandController.instance.objectsInHand[i].objectPrefab == null)
                    {
                        ObjectInHandController.instance.objectsInHand[i].objectPrefab = ObjectToPick;
                        ObjectInHandController.instance.objectsInHand[i].objectIcon = pickObjectIcon;
                        ObjectInHandController.instance.objectsInHand[i].objectName = pickObjectName;
                        InventoryMainController._instance.UpdateSell(i);
                        ObjectInHandController.instance.SetObject((sbyte)i);
                        Debug.LogError(i);
                        Destroy(gameObject);
                        return;
                    }
                    if(ObjectInHandController.instance.objectsInHand[i].objectName == pickObjectName)
                    {
                        ObjectInHandController.instance.objectsInHand[i].objectPrefab = ObjectToPick;
                        ObjectInHandController.instance.objectsInHand[i].objectIcon = pickObjectIcon;
                        ObjectInHandController.instance.objectsInHand[i].objectName = pickObjectName;
                        ObjectInHandController.instance.objectsInHand[i].count += 1;
                        InventoryMainController._instance.UpdateSell(i);
                        ObjectInHandController.instance.SetObject((sbyte)i);
                        Debug.LogError("количество - " + ObjectInHandController.instance.objectsInHand[i].count);
                        Destroy(gameObject);
                        return;
                    }
                }
                //Debug.LogError("Sdelano");
            }
        }
    }
}
