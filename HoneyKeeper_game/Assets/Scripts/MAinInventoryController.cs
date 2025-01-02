//using Microsoft.Unity.VisualStudio.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MAinInventoryController : ObjectInHandController
{
    public static MAinInventoryController Instance;
    [SerializeField] Transform _inventoryStart;
    [SerializeField] GameObject iconImage;
    public List<Image> slots = new List<Image>();
    int step = 120;
    Image child;
    int slotID;

    [SerializeField] public List<ObjectsInInventory> localObjects;
    void Start()
    {
        localObjects = objectsInHand;
        if (Instance == null)
            Instance = this;
        //child = transform;
        //GameObject _playerInventory = gameObject;//GameObject.Find("MainInventory---DNC---");
        //for (int i = 0; i < base.objectsInHand.Count; i++)
        //{
        //  child =
        //  Instantiate(iconImage, child.position + new Vector3(step, 0, 0), Quaternion.identity, _playerInventory.transform).transform;
        //
        //}
        //child = transform;
        //GameObject _playerInventory = gameObject.;//GameObject.Find("MainInventory---DNC---");
        for (int i = 0; gameObject.transform.GetChild(i) != null; i++)
        {
            child = gameObject.transform.GetChild(i).GetComponent<Image>();
            slots.Add(child);
        }
        //SetSlot(0);
    }
    public void SetSlot(int slotIndex)
   {
        for (int i = 0; i < slots.Count; i++)
        {
            if (i == slotIndex)
            {
                slots[i].color = Color.white;
            }
            else
            {
                slots[i].color = Color.gray;
            }
        }
    }

    public void UpdateSell(int slotToUpdate)
    {
        //localObjects = base.ObjectsInHand;
        slots[slotToUpdate].sprite = ObjectInHandController.instance.objectsInHand[slotToUpdate].objectIcon;
        Debug.LogError("UpdatingSell" + ObjectInHandController.instance.objectsInHand[slotToUpdate].objectIcon);
    }
}
