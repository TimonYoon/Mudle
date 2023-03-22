using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System.Linq;


[SerializeField]
public class ItemData
{
    public enum ItemType
    {
        Material,
        Food,
    }

    public ItemData(JsonData json)
    {
        materialID = JsonParser.ToString(json["id"]);
        name = JsonParser.ToString(json["name"]);
        imageName = JsonParser.ToString(json["imageName"]);
        string typeText = JsonParser.ToString(json["type"]);
        itemType = (ItemType)System.Enum.Parse((typeof(ItemType)), typeText);
    }

    public string materialID { get; private set; }
    public string name { get; private set; }
    public string imageName { get; private set; }
    public ItemType itemType { get; private set; }

}
public class GuestData
{
    public GuestData(JsonData json)
    {
        id = JsonParser.ToString(json["id"]);
        bodyImageName = JsonParser.ToString(json["bodyImageName"]);
        armImageName = JsonParser.ToString(json["armImageName"]);
    }

    public string id { get; private set; }

    public string bodyImageName { get; private set; }

    public string armImageName { get; private set; }


}

public class GameDataManagerTest : MonoBehaviour {

    Sprite[] myFruit;

    public static Dictionary<string, Sprite> materialSpriteDic = new Dictionary<string, Sprite>();
    public static Dictionary<string, ItemData> materialBaseDataDic = new Dictionary<string, ItemData>();
    public static Dictionary<string, OrderData> orderDataDic = new Dictionary<string, OrderData>();
    public static List<GuestData> guestDataList = new List<GuestData>();
    public static Dictionary<string, Sprite> guestSpriteDic = new Dictionary<string, Sprite>();

    private void Awake()
    {
        DontDestroyOnLoad(this);

        var file = Resources.Load<TextAsset>("JsonData/MaterialData");
        JsonData jsonData = JsonMapper.ToObject(file.text);
        for (int i = 0; i < jsonData.Count; i++)
        {
            ItemData data = new ItemData(jsonData[i]);
            materialBaseDataDic.Add(data.materialID, data);
        }

        var recipeJsonFile = Resources.Load<TextAsset>("JsonData/RecipeData");
        JsonData jsonRecipeData = JsonMapper.ToObject(recipeJsonFile.text);
        for (int i = 0; i < jsonRecipeData.Count; i++)
        {
            OrderData data = new OrderData(jsonRecipeData[i]);
            orderDataDic.Add(data.id, data);
        }

        var guestDataFile = Resources.Load<TextAsset>("JsonData/GuestData");
        JsonData jsonGuestData = JsonMapper.ToObject(guestDataFile.text);
        for (int i = 0; i < jsonGuestData.Count; i++)
        {
            GuestData data = new GuestData(jsonGuestData[i]);
            guestDataList.Add(data);
        }
        
        myFruit = Resources.LoadAll<Sprite>("Sprites/Material");
        
        for (int i = 0; i < myFruit.Length; i++)
        {
            //Debug.Log(myFruit[i].name);
            materialSpriteDic.Add(myFruit[i].name, myFruit[i]);
        }

        myFruit = Resources.LoadAll<Sprite>("Sprites/Cat");

        for (int i = 0; i < myFruit.Length; i++)
        {
            guestSpriteDic.Add(myFruit[i].name, myFruit[i]);           
        }
    }
}
