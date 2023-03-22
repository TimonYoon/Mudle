using UnityEngine;
using System.Collections;

public class SceneTest : MonoBehaviour {
    	
	void Awake ()
    {
        if(!GameController.Instance)
            UnityEngine.SceneManagement.SceneManager.LoadScene("Lobby");

    }
}
