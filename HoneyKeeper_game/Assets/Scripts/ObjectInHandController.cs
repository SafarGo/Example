using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectInHandController : MonoBehaviour
{
    public static ObjectInHandController instance { get; private set; }
    //public virtual List<ObjectsInInventory> ObjectsInHand { get => objectsInHand;  set => objectsInHand = value; }

    [Serializable]
    public class ObjectsInInventory
    {
        public string objectName;
        public int count = 0;
        public GameObject objectPrefab;
        public GameObject pickedObjectPrefab;
        public Sprite objectIcon;
    }

    public List<ObjectsInInventory> objectsInHand = new List<ObjectsInInventory> {};
    sbyte currentIndex;
    void Start()
    {
        if(instance == null)
            instance = this;


        //ChandgeObjectInHand(1);
    }


    void Update()
    {
        //if (StaticHolder.isCanFire)
        //{
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f)
        {
            ChandgeObjectInHand(-1);
        }
        else if (scroll < 0f)
        {
            ChandgeObjectInHand(1);
        }
        if(Input.GetKeyDown(KeyCode.G))
        {
            DropObjectInhand(currentIndex);
        }
        //}
    }

   public void ChandgeObjectInHand(sbyte index)
    {

        currentIndex += index;
        if (currentIndex >= 0 && currentIndex <= objectsInHand.Count - 1)
        {
            SetObject(currentIndex);
        }
        else if (currentIndex < 0)
        {
            currentIndex = (sbyte)(objectsInHand.Count - 1);
        }
        else
        {
            currentIndex = 0;
        }
        SetObject(currentIndex);
        //InventoryMainController._instance.SetSlot(currentIndex);
        //Debug.LogError(currentIndex);
    }

   public void SetObject(int exIndex)
    {
        InventoryMainController._instance.SetSlot(exIndex);
        for (int i = 0; i < objectsInHand.Count; i++)
        {
            if (i == exIndex)
            {
                if (objectsInHand[i].objectPrefab != null)
                    objectsInHand[i].objectPrefab.SetActive(true);
            }
            else
            {
                if (objectsInHand[i].objectPrefab != null)
                    objectsInHand[i].objectPrefab.SetActive(false);
            }
        }
        //currentIndex = (sbyte)exIndex;
        //MAinInventoryController.Instance.SetSlot(exIndex);
    }

    public void ClearSell(sbyte index)
    {
        if (objectsInHand[index].count < 1)
        {
            objectsInHand[index].objectPrefab.SetActive(false);
            objectsInHand[index].objectName = null;
            objectsInHand[index].count = 1;
            objectsInHand[index].objectPrefab = null;
            objectsInHand[index].pickedObjectPrefab = null;
            objectsInHand[index].objectIcon = null;
            InventoryMainController._instance.UpdateSell(index);
        }
    }

   public void DropObjectInhand(int ibjectID)
    {
        if (objectsInHand[ibjectID].objectPrefab != null)
        {
            objectsInHand[ibjectID].count--;
            if (objectsInHand[ibjectID].count < 1)
            {
                objectsInHand[ibjectID].pickedObjectPrefab.SetActive(true);
                Instantiate(objectsInHand[ibjectID].pickedObjectPrefab, objectsInHand[ibjectID].objectPrefab.transform.position, Quaternion.identity);
                Destroy(objectsInHand[ibjectID].pickedObjectPrefab);
                //objectsInHand[ibjectID].pickedObjectPrefab.SetActive(true);
                ClearSell((sbyte)ibjectID);
                objectsInHand[ibjectID].count = 1;
            }
            else
            {
                objectsInHand[ibjectID].pickedObjectPrefab.gameObject.SetActive(true);
                //GameObject predObject = 
                GameObject sledPickObj = Instantiate(objectsInHand[ibjectID].pickedObjectPrefab.gameObject, objectsInHand[ibjectID].objectPrefab.transform.position, Quaternion.identity);
                objectsInHand[ibjectID].pickedObjectPrefab.gameObject.SetActive(false);
                // Destroy(objectsInHand[ibjectID].pickedObjectPrefab);
                // objectsInHand[ibjectID].pickedObjectPrefab = sledPickObj;
                ClearSell((sbyte)ibjectID);
            }
            InventoryMainController._instance.SetCount(ibjectID);
        }
    }
}
