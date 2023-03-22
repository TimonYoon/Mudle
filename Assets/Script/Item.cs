using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum DragAndDropType
{
    Material,
    Food,
}

public class Item : MonoBehaviour
{
    public string id;

    public ItemData itemData;

    public bool isNotInitImage = false;
    private void Start()
    {
        InitImage();
    }

    public void InitImage()
    {

        if (!string.IsNullOrEmpty(id))
        {
            itemData = GameDataManagerTest.materialBaseDataDic[id];

            if (isNotInitImage)
                return;

            Image imageMaterial = GetComponent<Image>();

            if (!string.IsNullOrEmpty(itemData.imageName))
                imageMaterial.sprite = GameDataManagerTest.materialSpriteDic[itemData.imageName];
        }
    }
}
