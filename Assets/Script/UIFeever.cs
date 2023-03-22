using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFeever : MonoBehaviour
{
    public static UIFeever Instance;

    public GameObject feeverPanel;
    private void Awake()
    {
        feeverPanel.SetActive(true);
        Instance = this;
    }
   
    public void Show()
    {
        feeverPanel.SetActive(false);
        SoundManager.Play(SoundType.Feever);
    }

    public void Hide()
    {
        feeverPanel.SetActive(true);
    }
}
