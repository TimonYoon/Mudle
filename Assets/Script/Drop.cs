using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CookingData
{
    public CookingType cookingType = CookingType.None; 

    public List<string> itemIDList = new List<string>(); 
}
public class Drop : MonoBehaviour
{
    public delegate void OnDropItemCallback(ItemData item);
    public OnDropItemCallback onDropItemCallback;

    public delegate void OnDropCookingCallback(CookingData cooking);
    public OnDropCookingCallback onDropCookingCallback;

    public DragAndDropType canDropType;

    public void OnDrop(Item _item)
    {       
        ItemData item = _item.itemData;        
        string name = item.name;
        //Debug.Log(gameObject.name + " => Drop : " + name);

        if (onDropItemCallback != null)
        {
            onDropItemCallback(item);
        }
    }

    public void OnDrop(Cooking _cooking)
    {
        CookingData cookingData = new CookingData();
        cookingData.cookingType = _cooking.cookingType;
        cookingData.itemIDList = _cooking.itemIDList;

        if (onDropCookingCallback != null)
            onDropCookingCallback(cookingData);
    }
}
