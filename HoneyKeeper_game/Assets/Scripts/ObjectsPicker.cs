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
        Debug.LogError(isPlayerOn);
    }

    private void Update()
    {
        //Debug.LogError(isPlayerOn);
        if(!isPlayerOn)
        {
            return;
        }
        else
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
                        MAinInventoryController.Instance.UpdateSell(i);
                        //MAinInventoryController.Instance.UpdateSell(0);
                        Debug.LogError(i);
                        Destroy(gameObject);
                        return;
                    }
                }
                Debug.LogError("Sdelano");
            }
        }
    }
}
