using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HAndObjectsController : MonoBehaviour
{
    [SerializeField] private List<GameObject> objectsInHand = new List<GameObject> { null };
    sbyte currentIndex;
    void Start()
    {
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

   void ChandgeObjectInHand(sbyte index)
    {

            currentIndex += index;
        if (currentIndex >= 0 && currentIndex <= objectsInHand.Count - 1)
        {
            SetObject(currentIndex);
        }
        else if(currentIndex < 0)
        {
            currentIndex = (sbyte)(objectsInHand.Count - 1);
        }
        else
        {
            currentIndex = 0;
        }
        SetObject(currentIndex);
        Debug.LogError(currentIndex);
    }

    void SetObject(int exIndex)
    {
        for (int i = 0; i < objectsInHand.Count; i++)
        {
            if (i == exIndex)
            {
                if (objectsInHand[i] != null)
                    objectsInHand[i].SetActive(true);
            }
            else
            {
                if (objectsInHand[i] != null)
                    objectsInHand[i].SetActive(false);
            }
        }
    }

}
