using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOption : MonoBehaviour {

    public Toggle BGM_Toggle;
    public Toggle ES_Toggle;
    public Toggle Vibrate_Toggle;

    private void OnEnable()
    {
        BGM_Toggle.isOn = !OptionManager.Instance.isBGM;
        ES_Toggle.isOn = !OptionManager.Instance.isEffectSound;
        Vibrate_Toggle.isOn = !OptionManager.Instance.isVibrate;
    }

    public void OnClickBGM()
    {
        //BGM_Toggle.isOn = !BGM_Toggle.isOn;
        OptionManager.Instance.SetBGM();
    }

    public void OnClickES()
    {
        //ES_Toggle.isOn = !ES_Toggle.isOn;
        OptionManager.Instance.SetEffectSound();
    }

    public void OnClickVibrate()
    {
        //Vibrate_Toggle.isOn = !Vibrate_Toggle.isOn;
        OptionManager.Instance.SetVibrate();
    }


}
