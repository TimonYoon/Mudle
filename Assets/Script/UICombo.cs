using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class UICombo : MonoBehaviour {

    public static UICombo Instance;

    public GameObject panel;

    // 0~ 9까지 있음
    public List<Sprite> numberList = new List<Sprite>();

    // 1 자리 부터 있음
    public List<Image> imageList = new List<Image>();

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        Hide();
        //Show(1234);
    }
    public void Show(int comboCount)
    {
        if (comboCount < 2)
        {
            Hide();
            return;
        }
        

        List<int> numList = new List<int>();

        int _value = comboCount;

        while (true)
        {
            int a = _value / 10;
            int b = _value % 10;
            _value = a;
            numList.Add(b);
            if (_value == 0)
                break;
        }
        
        if(imageList.Count <= numList.Count)
        {
            for (int i = 0; i < imageList.Count; i++)
            {
                imageList[i].gameObject.SetActive(true);
                imageList[i].sprite = numberList[9];
            }
            // 99999로 표기
        }
        else
        {
            for (int i = 0; i < numList.Count; i++)
            {
                int num = numList[i];
                imageList[i].gameObject.SetActive(true);
                imageList[i].sprite = numberList[num];
            }
        }
        SoundManager.Play(SoundType.Count);

        panel.SetActive(true);

    }

    public void Hide()
    {
        panel.SetActive(false);
        for (int i = 0; i < imageList.Count; i++)
        {
            imageList[i].gameObject.SetActive(false);
        }
    }
}
