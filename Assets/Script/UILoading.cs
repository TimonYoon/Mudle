using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILoading : MonoBehaviour {

    public static UILoading Instance;
    Animator anim;

    private void Awake()
    {
        Instance = this;
        anim = GetComponent<Animator>();
    }

    public void Close()
    {
        anim.SetTrigger("Close");
    }

    public void Open()
    {
        anim.SetTrigger("Open");
    }
}
