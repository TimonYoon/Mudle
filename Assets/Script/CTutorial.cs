using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTutorial : MonoBehaviour {
    public static CTutorial Instance;
    public GameObject panel;
    private void Awake()
    {
        Instance = this;
    }
    public void Show()
    {
        Time.timeScale = 0;
        panel.SetActive(true);
    }
    public void Hide()
    {
        Time.timeScale = 1;
        panel.SetActive(false);
    }
}
