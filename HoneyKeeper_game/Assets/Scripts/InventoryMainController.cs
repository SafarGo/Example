using System.Collections;
using System.Collections.Generic;
using TMPro;
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


        childObj = gameObject.transform.GetChild(0).gameObject;
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {

            child = gameObject.transform.GetChild(i).GetComponent<Image>();
            slots.Add(child);
        }

    }

    public void SetSlot(int slotIndex)
    {
        //slots[7].color = Color.grey;
        for (int i = 0; i < slots.Count; i++)
        {

            if (i == slotIndex)
            {
                slots[i].color = Color.white;
            }
            else
            {
                slots[i].color = Color.grey;
            }
            SetCount(i);
        }
        //slots[slotIndex].color = Color.gray;

    }

    public void UpdateSell(int slotToUpdate)
    {
        //localObjects = base.ObjectsInHand;
        slots[slotToUpdate].sprite = ObjectInHandController.instance.objectsInHand[slotToUpdate].objectIcon;
        Debug.LogError("UpdatingSell" + ObjectInHandController.instance.objectsInHand[slotToUpdate].objectIcon);
    }

    public void SetCount(int index)
    {
        slots[index].transform.GetChild(0).GetComponent<TMP_Text>().text = ObjectInHandController.instance.objectsInHand[index].count.ToString();
    }
}
