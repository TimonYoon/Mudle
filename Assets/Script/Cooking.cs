using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CookingType
{
    None, //미완성
    Perfect, // 완성
    Fail, // 실패(시간초과)
    Miss, 
}

public class Cooking : MonoBehaviour {

    public List<Item> materialList = new List<Item>();

    public GameObject noWaterInSoup;
    Drop drop;

    Item item;

    public GameObject failImage;
    public GameObject normalImage;


    CookingType _cookingType = CookingType.None;
    public CookingType cookingType
    {
        get { return _cookingType; }
        private set
        {
            if(_cookingType != value)
            {
                _cookingType = value;
                if (_cookingType == CookingType.Perfect)
                    SoundManager.Play(SoundType.PrefectFood);
            }
        }
    }


    public GameObject goPerfect;
    public GameObject goFire;

    private void Awake()
    {
        if(GameController.Instance)
        {
            GameController.Instance.onGoToHome += OnGoToHome;
            GameController.Instance.onGameReStart += OnGameReStart;
        }
    }
    void OnGoToHome()
    {
        GameController.Instance.onGoToHome -= OnGoToHome;
        GameController.Instance.onGameReStart -= OnGameReStart;
    }
    void OnGameReStart()
    {
        CookingClear();
    }


    void Start()
    {
        item = GetComponent<Item>();
        drop = GetComponent<Drop>();

        if(drop != null)
            drop.onDropItemCallback += OnDropItem;

        for (int i = 0; i < materialList.Count; i++)
        {
            materialList[i].gameObject.SetActive(false);
        }
        noWaterInSoup.SetActive(false);
        normalImage.SetActive(false);
    }


    // 해당 요리에 첨가된 재료 리스트
    public List<ItemData> dropItemDataList = new List<ItemData>();

    public float cookingTime;

    public List<string> itemIDList;
    void OnDropItem(ItemData item)
    {
        for (int i = 0; i < dropItemDataList.Count; i++)
        {
            if (dropItemDataList[i].materialID == item.materialID)
                return;
        }

        // to do : 라면 완성 시간이후 넣은 재료 인정할 것인가??
        //if (cookingType != CookingType.None)
        //    return;

        if (cookingType == CookingType.Fail)
            return;

        bool isWater = false;
        bool isInTheSoup = false;
        for (int i = 0; i < dropItemDataList.Count; i++)
        {
            if (dropItemDataList[i].materialID == "material_001")
            {
                isWater = true;
            }

            if(dropItemDataList[i].materialID == "material_003")
            {
                isInTheSoup = true;
            }

            
        }

        if(item.materialID == "material_001")
            isWater = true;

        if (item.materialID == "material_003")
            isInTheSoup = true;

        
        for (int i = 0; i < materialList.Count; i++)
        {
            if (materialList[i].id == "material_003")
            {
                noWaterInSoup.SetActive(!isWater && isInTheSoup);
                materialList[i].gameObject.SetActive(isWater && isInTheSoup);
            }

            if (materialList[i].id == item.materialID)
            {
                if (materialList[i].id == "material_003")
                {
                    continue;
                }
                else
                {
                    materialList[i].gameObject.SetActive(true);
                }
                
                //break;

            }
        }
        normalImage.SetActive(materialList.Count > 0);


        dropItemDataList.Add(item);
        itemIDList.Add(item.materialID);
    }
    float startTime;
    private void Update()
    {
        if (dropItemDataList.Count == 0)
        {
            goFire.SetActive(false);
            goPerfect.SetActive(false);
            cookingTime = 0;
            cookingType = CookingType.None;
            startTime = Time.time;
            return;
        }
        goFire.SetActive(true);
        //if (GameController.Instance.isPause)
        //    return;

        cookingTime += Time.deltaTime;
        



        if(cookingTime > 8.5)
        {
            cookingType = CookingType.Fail;

            //CookingClear();
            failImage.SetActive(true);
            item.id = "fail";            
            //item.InitImage();
            // 실패 (시간초과)
        }
        else if(cookingTime >5)
        {
            cookingType = CookingType.Perfect;
            item.id = "food_001";
            
            //
            //for (int i = 0; i < materialList.Count; i++)
            //{
            //    materialList[i].gameObject.SetActive(false);
            //}
            // 완성
        }
        goPerfect.SetActive(cookingType == CookingType.Perfect);
    }

    public void CookingClear()
    {
        normalImage.SetActive(false);
        dropItemDataList.Clear();
        itemIDList.Clear();
        for (int i = 0; i < materialList.Count; i++)
        {
            materialList[i].gameObject.SetActive(false);
        }
        noWaterInSoup.SetActive(false);
        failImage.SetActive(false);
    }


}
