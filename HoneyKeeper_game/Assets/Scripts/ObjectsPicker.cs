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

    private void Awake()
    {
        if(ObjectToPick == null)
        {
            ObjectToPick = ObjectInHandController.instance.gameObject.transform.Find(pickObjectName).gameObject;
            //ObjectToPick.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            isPlayerOn = true;
        }
        //Debug.LogError(isPlayerOn);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isPlayerOn = false;
        }

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
                        ObjectInHandController.instance.objectsInHand[i].pickedObjectPrefab = gameObject;
                        InventoryMainController._instance.UpdateSell(i);
                        ObjectInHandController.instance.SetObject((sbyte)i);/////
                        Debug.LogError(i);
                        gameObject.SetActive(false);//
                        //Destroy(gameObject);
                        InventoryMainController._instance.SetCount(i);
                        return;
                    }
                    if(ObjectInHandController.instance.objectsInHand[i].objectName == pickObjectName)
                    {
                        ObjectInHandController.instance.objectsInHand[i].objectPrefab = ObjectToPick;
                        ObjectInHandController.instance.objectsInHand[i].objectIcon = pickObjectIcon;
                        ObjectInHandController.instance.objectsInHand[i].objectName = pickObjectName;
                        ObjectInHandController.instance.objectsInHand[i].count += 1;
                        InventoryMainController._instance.UpdateSell(i);
                        ObjectInHandController.instance.SetObject((sbyte)i);////
                        Debug.LogError("количество - " + ObjectInHandController.instance.objectsInHand[i].count);
                        Destroy(gameObject);
                        //Destroy(gameObject);
                    InventoryMainController._instance.SetCount(i);
                        return;
                    }
                }
                //Debug.LogError("Sdelano");
            }
        }
    }
}
