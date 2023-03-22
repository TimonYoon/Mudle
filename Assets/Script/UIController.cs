using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class UIController : MonoBehaviour {

    public Image imageGameTimeProgressBar;
    public Text textGameTime;
    public Text textGameScore;

    float origialProgressBarWidthSize;
    private void Start()
    {
        goResultPopup.SetActive(false);
        origialProgressBarWidthSize = imageGameTimeProgressBar.rectTransform.sizeDelta.x;

        GameController.Instance.onChangedGameTimeCallback += OnChangedGameTimeCallback;
        GameController.Instance.onChangedGameScoreCallback += OnChangedGameScoreCallback;
        GameController.Instance.onGameEndCallback += OnGameEndCallback;
        //Debug.Log("게임 UI 시작");
        StartCoroutine(GameStart());
        textGameTime.text = GameController.Instance.maxGameTime.ToString("N0");
        textGameScore.text = GameController.Instance.score.ToString("N0");
    }
    /// <summary> 1,234,567 이런 형식의 콤마를 붙인 텍스트로 바꿔줌 </summary>
    public string ToStringComma( double number)
    {

        string returnData = string.Empty;

        string data = number.ToString();

        char[] charArray = data.ToCharArray().Reverse().ToArray();

        for (int i = 0; i < charArray.Length; i++)
        {
            returnData = charArray[i] + returnData;
            if ((i + 1) % 3 == 0 && (i + 1) < charArray.Length)
                returnData = ',' + returnData;
        }
        return returnData;
    }
    void OnChangedGameTimeCallback(float time)
    {
        float value = time / GameController.Instance.maxGameTime;
        textGameTime.text = time.ToString("N0");
        imageGameTimeProgressBar.rectTransform.sizeDelta = (Vector2.right * origialProgressBarWidthSize * value) + (Vector2.up * imageGameTimeProgressBar.rectTransform.sizeDelta.y);
    }

    void OnChangedGameScoreCallback(double score)
    {
        textGameScore.text = ToStringComma(score);
    }
    public GameObject goReady;
    public GameObject goStart;
    
    IEnumerator GameStart()
    {
        //Debug.Log("게임 카운드 시작 = " + GameController.Instance.isStart);
        goReady.SetActive(!GameController.Instance.isStart);
        goStart.SetActive(GameController.Instance.isStart);
        while (GameController.Instance.isStart == false)
        {
            yield return null;
        }

        goReady.SetActive(!GameController.Instance.isStart);
        SoundManager.Play(SoundType.CatSound);
        goStart.SetActive(GameController.Instance.isStart);

        Invoke("Test", 1.5f);

    }
    void Test()
    {
        goStart.SetActive(false);
    }

    void OnGameEndCallback()
    {
        SoundManager.Play(SoundType.Result);
        goResultPopup.SetActive(true);
    }
    public GameObject goResultPopup;
    public void OnClickGoToLobby()
    {
        OnClickPauseClose();
        GameController.Instance.isGameEnd = true;
        GameController.Instance.GoToHome();
       
        GameController.Instance.onChangedGameTimeCallback -= OnChangedGameTimeCallback;
        GameController.Instance.onChangedGameScoreCallback -= OnChangedGameScoreCallback;
        GameController.Instance.onGameEndCallback -= OnGameEndCallback;
        
        string sceneName = "InGame";
        SceneManager.UnloadSceneAsync(sceneName);
    }

    public void OnClickReStrat()
    {        
        GameController.Instance.GameReStart();
        OrderManager.Instance.Restart();
        goResultPopup.SetActive(false);
        StartCoroutine(GameStart());
    }

    public void OnClickPause()
    {
        Time.timeScale = 0;
        SoundManager.BGM_Mute(true);
        
    }

    public void OnClickPauseClose()
    {
        Time.timeScale = 1;
        SoundManager.BGM_Mute(false);
    }

    string subject = "MEOWDLE";
    string body = "https://play.google.com/store/apps/details?id=com.blackout.plojectA";
    Coroutine shareCoroutine;
    public void OnClickShareButton()
    {
        if (shareCoroutine != null)
            return;
        shareCoroutine = StartCoroutine(ShareCoroutine());
        Debug.Log("Share Start");
    }
    IEnumerator ShareCoroutine()
    {

        yield return new WaitForEndOfFrame();
#if UNITY_ANDROID

        Debug.Log("Share 1");
        AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
        AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");


        intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));

        AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
        //AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "file://" + destination);

        //intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);
        intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), "MEOWDLE");
        //intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), ShareSubject);
        intentObject.Call<AndroidJavaObject>("setType", "image/png");

        Debug.Log("Share 2");
        intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"),subject);

        //intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), body);
        Debug.Log("Share 2-1");

        AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject jChooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, "Share Via");
        Debug.Log("Share 2-2");
        currentActivity.Call("startActivity", jChooser);

        Debug.Log("Share 3");
#endif
        shareCoroutine = null;
        yield break;
    }
}
