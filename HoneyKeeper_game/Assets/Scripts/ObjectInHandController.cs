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
        public Sprite objectIcon;
    }

    public List<ObjectsInInventory> objectsInHand = new List<ObjectsInInventory> {};
    sbyte currentIndex;
    void Start()
    {
        if(instance == null)
            instance = this;
        ChandgeObjectInHand(0);
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
        //MAinInventoryController.Instance.SetSlot(exIndex);
    }

    public void DiscardSell(sbyte index)
    {
        objectsInHand[index].objectPrefab.SetActive(false);
        objectsInHand[index].objectName = null;
        objectsInHand[index].count = 0;
        objectsInHand[index].objectPrefab = null;
        objectsInHand[index].objectIcon = null;
        InventoryMainController._instance.UpdateSell(index);
    }
}
