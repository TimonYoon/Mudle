using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LobbyManager : MonoBehaviour {

    public static LobbyManager Instance;

    public delegate void OnLobbyCallback();
    public OnLobbyCallback onGameSceneLoadSuccess;


    private void Awake()
    {
        if (Instance != null)
            Destroy(this);
        else
            Instance = this;
    }

    Coroutine gameStartCoroutine;
    public void OnClickGameStartButton()
    {
        if (gameStartCoroutine != null)
            return;

        gameStartCoroutine = StartCoroutine(SceneLoad());
    }

    IEnumerator SceneLoad()
    {
        UILoading.Instance.Close();

        yield return new WaitForSeconds(0.5f);
        string sceneName = "InGame";
        Scene scene = SceneManager.GetSceneByName(sceneName);

        if (!scene.isLoaded)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);

            while (!scene.isLoaded)
            {
                scene = SceneManager.GetSceneByName(sceneName);
                yield return null;
            }           
        }

        UILoading.Instance.Open();

        if (onGameSceneLoadSuccess != null)
            onGameSceneLoadSuccess();       

        gameStartCoroutine = null;
    }
}
