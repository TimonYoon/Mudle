using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;


public class OrderData
{
    public OrderData(JsonData json)
    {
        id = json["id"].ToString();
        level = int.Parse(json["level"].ToString());
        name = json["name"].ToString();
        for (int i = 0; i < 9; i++)
        {
            string key = "materialID_" + (i + 1);
            
            string materialID = json[key].ToString();
            if(!string.IsNullOrEmpty(materialID))
            {
                needMaterialIDList.Add(materialID);
            }
        }
        //Debug.Log(name + " / " + needMaterialIDList.Count);
    }

    public bool OrderChack(List<string> dropMaterialIDList)
    {
        bool isResult = false;

        List<string> _needMaterialIDList = new List<string>();
        for (int i = 0; i < needMaterialIDList.Count; i++)
        {
            _needMaterialIDList.Add(needMaterialIDList[i]);
        }

        int needMaterialCount = _needMaterialIDList.Count;

        if(needMaterialCount == dropMaterialIDList.Count)
        {
            for (int i = 0; i < dropMaterialIDList.Count; i++)
            {
                for (int j = 0; j < _needMaterialIDList.Count; j++)
                {
                    if (_needMaterialIDList[j] == dropMaterialIDList[i])
                    {
                        _needMaterialIDList.RemoveAt(j);
                        needMaterialCount--;
                        break;
                    }
                }
            }
        }
       

        isResult = needMaterialCount == 0;

        return isResult;
    }

    public string id { get; private set; }
    public string name { get; private set; }
    public int level { get; private set; }
    public List<string> needMaterialIDList = new List<string>();
}
