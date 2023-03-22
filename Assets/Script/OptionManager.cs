using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionManager : MonoBehaviour {

    public static OptionManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    string key = "";
    private void Start()
    {
        key = "vibrate";
        if (PlayerPrefs.HasKey(key))
        {
            string data = PlayerPrefs.GetString(key);
            isVibrate = bool.Parse(data);
        }

        key = "bgm";
        if (PlayerPrefs.HasKey(key))
        {
            string data = PlayerPrefs.GetString(key);
            isBGM = bool.Parse(data);
        }

        key = "effectSound";
        if (PlayerPrefs.HasKey(key))
        {
            string data = PlayerPrefs.GetString(key);
            isEffectSound = bool.Parse(data);
        }

        if (isBGM)
            SoundManager.BGM_Play();
    }

    public bool isVibrate = true;
    public bool isBGM = true;
    public bool isEffectSound = true;

    public void SetVibrate()
    {
        isVibrate = !isVibrate;
        key = "vibrate";
        PlayerPrefs.SetString(key, isVibrate.ToString());
    }

    public void SetBGM()
    {
        isBGM = !isBGM;
        key = "bgm";
        PlayerPrefs.SetString(key, isBGM.ToString());

        if (isBGM)
            SoundManager.BGM_Play();
        else
            SoundManager.BGM_Stop();

    }

    public void SetEffectSound()
    {
        isEffectSound = !isEffectSound;
        key = "effectSound";
        PlayerPrefs.SetString(key, isEffectSound.ToString());
    }

    public void Vibrate()
    {
        if(isVibrate)
            Handheld.Vibrate();
    }

    
}
