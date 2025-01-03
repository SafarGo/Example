using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static ObjectInHandController;

public class InventoryMainController : ObjectInHandController
{
    public static InventoryMainController _instance;
    [SerializeField] Transform _inventoryStart;
    [SerializeField] GameObject iconImage;
    public List<Image> slots = new List<Image>();
    int step = 120;
    Image child;
    int slotID;
    GameObject childObj;

    public List<ObjectsInInventory> localObjects;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }
    void Start()
    {
        localObjects = objectsInHand;

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
        //SetSlot(5);
        childObj = gameObject.transform.GetChild(0).gameObject;
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            //if(gameObject.transform.GetChild(i) != null)
            //childObj = gameObject.transform.GetChild(i).gameObject;
            child = gameObject.transform.GetChild(i).GetComponent<Image>();
            slots.Add(child);
        }
        //SetSlot(5);
    }

    public void SetSlot(int slotIndex)
    {
        //slots[7].color = Color.grey;
        for (int i = 0; i < slots.Count; i++)
        {
            //if (i == slotIndex)
            //{
            //    slots[i].color = Color.white;
            //}
            //else
            //{
            //    slots[i].color = Color.gray;
            //}
            //if (i == slotIndex)
            //{
            //    slots[i].color = Color.grey;
            //}
            //slots[i].color = Color.grey;
            // if(i == slotIndex)
            // slots[i].color = Color.grey;
            // if(i != slotIndex)
            // {
            //     slots[i].color = Color.grey;
            // }
            //else
            //{
            //    slots[i].color = Color.white;
            //}
            // Debug.LogError( "Индекс  " + slotIndex + "    i   " + i);
            if (i == slotIndex)
            {
                slots[i].color = Color.white;
            }
            if(i != slotIndex)
            {
                slots[i].color = Color.grey;
            }
            //else
            //{
            //    slots[slotIndex].color = Color.white;
            //}
            //Debug.LogError("выполнено   " + i + "   раз");
        }
        //slots[slotIndex].color = Color.gray;

    }

    public void UpdateSell(int slotToUpdate)
    {
        //localObjects = base.ObjectsInHand;
        slots[slotToUpdate].sprite = ObjectInHandController.instance.objectsInHand[slotToUpdate].objectIcon;
        Debug.LogError("UpdatingSell" + ObjectInHandController.instance.objectsInHand[slotToUpdate].objectIcon);
    }
}
