using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class OrderManager : MonoBehaviour {

    public static OrderManager Instance;

    public List<Guest> guestList = new List<Guest>();

    public List<OrderData> orderDataList = new List<OrderData>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        orderDataList = GameDataManagerTest.orderDataDic.Values.ToList();
        Init();
    }
    public void Init()
    {
        GameController.Instance.onGameStartCallback += OnGameStartCallback;
        GameController.Instance.onGameEndCallback += OnGameEndCallback;
    }
    public void Restart()
    {
        //Init();
        for (int i = 0; i < guestList.Count; i++)
        {
            guestList[i].ReStart();
        }
    }

    void OnGameStartCallback()
    {
        if (gameStartCoroutine != null)
            return;

        gameStartCoroutine = StartCoroutine(GameStartCoroutine());
    }

    void OnGameEndCallback()
    {
        if (gameStartCoroutine != null)
        {
            StopCoroutine(gameStartCoroutine);
            gameStartCoroutine = null;
        }

    }

    public void GoToHome()
    {
        GameController.Instance.onGameStartCallback -= OnGameStartCallback;
        GameController.Instance.onGameEndCallback -= OnGameEndCallback;
    }
    public float minSpeed = 0.5f;
    float speed = 1.5f;

    Coroutine gameStartCoroutine;
    IEnumerator GameStartCoroutine()
    {
        // 주문간에 최소 간격과 최대 간격이 존재
        // 난이도 높은 것과 낮은 것이 존재
        // 

        float playTime = 0;

        while(true)
        {
            playTime += Time.deltaTime;

            speed = 1.5f - (playTime * 0.1f * 8);
            //Debug.Log("speed : " + speed);
            if (speed < minSpeed)
                speed = minSpeed;

            for (int i = 0; i < guestList.Count; i++)
            {
                if (guestList[i].isInitGuest == false)
                {
                    guestList[i].InitGuest(ReturnToOrderData());
                    break;
                }
                    
            }
            yield return new WaitForSeconds(speed);
            yield return null;
        }
        
    }

    OrderData ReturnToOrderData()
    {
        OrderData data = null;
        Debug.Log("오버 생성 레벨 : " + GameController.Instance.gameLevel);
        List<OrderData> list = orderDataList.FindAll(x => x.level <= GameController.Instance.gameLevel);
        int orderCount = list.Count;
        int index = Random.Range(0, orderCount);
        //Debug.Log("index : " + index);
        data = list[index];

        return data;
    }


}
