using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Guest : MonoBehaviour {

    Drop drop;
    Order order;
    OrderData orderData;
    public RectTransform guestTr;
    public Image imageGuest;
    public Image imageHand;
    public GameObject goNoodle;
    public GameObject goEmoji;
    public float hideGuestY = -150;
    public float speed = 5;
    void Start ()
    {
        guestTr.anchoredPosition = Vector2.up * hideGuestY;
        drop = GetComponent<Drop>();
        order = GetComponent<Order>();

        if (drop != null)
            drop.onDropCookingCallback += OnDropItem;
    }

    public void ReStart()
    {
        order.HideOrder();
        HideGuest();
        
    }

    void OnDropItem(CookingData cookingData)
    {
        if(cookingData.cookingType == CookingType.Perfect)
        {
            if(orderData.OrderChack(cookingData.itemIDList) == false)
            {         
                cookingData.cookingType = CookingType.Miss;
            }          
        }

        if (GameController.Instance.isFeever)
        {
            if (orderData.OrderChack(cookingData.itemIDList))
            {
                cookingData.cookingType = CookingType.Perfect;
                Debug.Log("피버 모드 재료만 잘 들어감");
            }
            else
            {
                cookingData.cookingType = CookingType.Miss;
                Debug.Log("피버 모드 재료 실패 ㅜㅜ ");
            }
                
        }

        GameController.Instance.Result(cookingData.cookingType);
        order.ResultOrder(cookingData.cookingType);

        if (cookingData.cookingType == CookingType.Perfect)
            goNoodle.SetActive(true);
        else
            goEmoji.SetActive(true);
        HideGuest();
    }

    public bool isInitGuest { get; private set; }
    public void InitGuest(OrderData _orderData)
    {
        if (isInitGuest)
            return;
        isInitGuest = true;
        if (_orderData == null)
            Debug.Log("orderData null!!!");
        orderData = _orderData;

        int index = Random.Range(0, GameDataManagerTest.guestDataList.Count);
        GuestData data = GameDataManagerTest.guestDataList[index];
        imageGuest.sprite = GameDataManagerTest.guestSpriteDic[data.bodyImageName];
        imageHand.sprite = GameDataManagerTest.guestSpriteDic[data.armImageName];
        imageHand.gameObject.SetActive(false);
        ShowGuest(); 
    }

    void ShowGuest()
    {
        StartCoroutine(ShowGuestCoroutine());
    }
    IEnumerator ShowGuestCoroutine()
    {
        while(guestTr.anchoredPosition.y < 0)
        {
            guestTr.anchoredPosition = Vector2.MoveTowards(guestTr.anchoredPosition, Vector2.zero, speed * Time.deltaTime);
            yield return null;
        }

        order.ShowOrder(orderData);

    }

    Coroutine hideGuestCoroutine;
    void HideGuest()
    {
        if (hideGuestCoroutine != null)
            return;

        hideGuestCoroutine = StartCoroutine(HideGuestCoroutine());
    }
    IEnumerator HideGuestCoroutine()
    {
        imageHand.gameObject.SetActive(true);
        while (guestTr.anchoredPosition.y > hideGuestY)
        {
            guestTr.anchoredPosition = Vector2.MoveTowards(guestTr.anchoredPosition, Vector2.up * hideGuestY, speed * Time.deltaTime);
            yield return null;
        }
        orderData = null;
        isInitGuest = false;
        order.HideOrder();
        goNoodle.SetActive(false);
        goEmoji.SetActive(false);
        hideGuestCoroutine = null;
    }
}
