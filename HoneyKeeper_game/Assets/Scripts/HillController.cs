using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HillController : MonoBehaviour
{
    [SerializeField] string hillName;
    [SerializeField] byte hillWeght;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            for(int i = 0; i < ObjectInHandController.instance.objectsInHand.Count; i++)
            {

            if (ObjectInHandController.instance.objectsInHand[i].objectName == hillName)
                {
                   if( ObjectInHandController.instance.objectsInHand[i].count > 0)
                   {
                        ObjectInHandController.instance.objectsInHand[i].count--;
                        InventoryMainController._instance.SetCount(i);
                        PlayerLifeController.instance.UpdateLife(hillWeght);
                        //InventoryMainController._instance.SetCount(ObjectInHandController.instance.objectsInHand[i].count);
                        if(ObjectInHandController.instance.objectsInHand[i].count < 1)
                            ObjectInHandController.instance.ClearSell((sbyte)i);
                    }
                   // else
                   // {
                   //     ObjectInHandController.instance.ClearSell((sbyte)i);
                   // }
                }
            }
        }
    }
}
