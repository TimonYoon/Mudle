using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order : MonoBehaviour {

    public GameObject goOrder;
    public List<Item> materialList = new List<Item>();
    public GameObject goFail;

    public GameObject goMiss;
    public GameObject goPerfact;
    public GameObject goNone;

    private void Start()
    {
        HideOrder();
    }
    public void ShowOrder(OrderData orderData)
    {
        for (int i = 0; i < materialList.Count; i++)
        {
            materialList[i].gameObject.SetActive(false);
        }
        goOrder.SetActive(true);
        for (int i = 0; i < orderData.needMaterialIDList.Count; i++)
        {
            string id = orderData.needMaterialIDList[i];

            for (int j = 0; j < materialList.Count; j++)
            {
                if (materialList[j].id == id)
                    materialList[j].gameObject.SetActive(true);
            }
        }
    }

    public void ResultOrder(CookingType cookingType)
    {
        goOrder.SetActive(false);
        //goFail.SetActive(cookingType == CookingType.Fail);
        //goMiss.SetActive(cookingType == CookingType.Miss);
        //goNone.SetActive(cookingType == CookingType.None);
        //goPerfact.SetActive(cookingType == CookingType.Perfect);
    }



    public void HideOrder()
    {
        goOrder.SetActive(false);
        goFail.SetActive(false);
        goMiss.SetActive(false);
        goNone.SetActive(false);
        goPerfact.SetActive(false);
        
    }
}
