using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICloseEye : MonoBehaviour {

    public float minCloseEyeTime = 2f;
    public float maxCloseEyeTime = 3.5f;
    Animator anim;

    private IEnumerator Start()
    {
        anim = GetComponent<Animator>();
        while (true)
        {
            float time = Random.Range(minCloseEyeTime, maxCloseEyeTime);           
            yield return new WaitForSeconds(time);

            anim.SetTrigger("CloseEye");
        }
    }


    void Update () {
		
	}
}
