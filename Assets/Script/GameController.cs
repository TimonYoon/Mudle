using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public delegate void OnGameStartCallback();
    public OnGameStartCallback onGameStartCallback;

    public delegate void OnGameEndCallback();
    public OnGameEndCallback onGameEndCallback;


    public delegate void OnGame();
    public OnGame onGameReStart;
    public OnGame onGoToHome;

    public delegate void OnChangedGameTimeCallback(float gameTime);
    public OnChangedGameTimeCallback onChangedGameTimeCallback;


    public delegate void OnChangedGameScoreCallback(double scoer);
    public OnChangedGameScoreCallback onChangedGameScoreCallback;

    double _score = 0;
    public double score
    {
        get { return _score; }
        set
        {
            _score = value;
            if (onChangedGameScoreCallback != null)
                onChangedGameScoreCallback(_score);
        }
    }

    /// <summary> 인게임 시간 </summary>
    public float maxGameTime = 60;

    float _gameTime;
    public float gameTime
    {
        get { return _gameTime; }
        private set
        {
            if (maxGameTime <= value)
                _gameTime = maxGameTime;
            else
                _gameTime = value;

            if (onChangedGameTimeCallback != null)
                onChangedGameTimeCallback(_gameTime);
        }
    }


    public bool isStart = false;
    public float readyTime = 2f;


    public bool isPause = false;

    private void Awake()
    {
        Instance = this;
        //if (Instance != null)
        //    Destroy(this);
        //else

    }

    public bool isBestScore = false;
    public double bestScore = 0;

    bool isFirst = true;

    private void Start()
    {
        string key = "bestScore";
        if(PlayerPrefs.HasKey(key))
        {
            bestScore = double.Parse(PlayerPrefs.GetString(key));
        }
        key = "isFirst";
        if (PlayerPrefs.HasKey(key))
        {
            string data = PlayerPrefs.GetString(key);
            isFirst = bool.Parse(data);
        }
        //Screen.SetResolution(720, 1280, false);

        LobbyManager.Instance.onGameSceneLoadSuccess += OnGameSceneLoadSuccess;
    }
    public bool isFeever = false;
    public int addTimeComboCount = 3;
    public float addTime = 6;
    public int feeverCount = 10;
    public float maxFeeverTime = 10;
    public float addFeeverTime = 5;
    float _feeverTime;

    int _combo = 0;
    public int combo
    {
        get { return _combo; }
        private set
        {
            _combo = value;
            if(_combo > 1)
            {
                if (_combo % addTimeComboCount == 0)
                    gameTime += addTime;
                if (_combo % feeverCount == 0)
                {
                    if(isFeever)
                        _feeverTime = addFeeverTime;
                    else
                        _feeverTime = maxFeeverTime;

                    isFeever = true;

                    if (isFeever)
                        UIFeever.Instance.Show();

                }
            }
            else
            {
                _feeverTime = 0;
            }
           

            if(UICombo.Instance)
                UICombo.Instance.Show(_combo);
        }
    }
    public void Result(CookingType type)
    {
        if (type == CookingType.Perfect)
        {
            SoundManager.Play(SoundType.PrefectMeow);
            combo++;
        }           
        else
        {
            OptionManager.Instance.Vibrate();
            combo = 0;
            SoundManager.Play(SoundType.FailMeow);
        }
            

       
            
        switch (type)
        {
            case CookingType.None:
                score += 100;
                break;
            case CookingType.Perfect:
                if (isFeever)
                    score += 600;
                else
                    score += 300;
                break;
            case CookingType.Fail:
                score += 50;
                break;
            case CookingType.Miss:
                score += 200;
                break;
            default:
                break;
        }
    }
    Coroutine gameStartCoroutine;

    public void GameReStart()
    {
        if (onGameReStart != null)
            onGameReStart();
        OnGameSceneLoadSuccess();
    }
    public void GoToHome()
    {
        if (onGoToHome != null)
            onGoToHome();

        OrderManager.Instance.GoToHome();

    }

    void OnGameSceneLoadSuccess()
    {
        Debug.Log("게임 시작");
        isStart = false;
        if(gameStartCoroutine == null)
            gameStartCoroutine = StartCoroutine(GameStart());
    }

    public bool isGameEnd = false;
    public int gameLevel = 1;
    IEnumerator GameStart()
    {
        if (isFirst)
        {
            isFirst = false;
            CTutorial.Instance.Show();
            PlayerPrefs.SetString("isFirst", isFirst.ToString());
        }

        gameLevel = 1;
        isBestScore = false;
        score = 0;
        combo = 0;
        UIFeever.Instance.Hide();
        float _readyTime = readyTime;
        Debug.Log(isStart + " / " + (_readyTime > 0));
        while (_readyTime > 0)
        {
            _readyTime -= Time.deltaTime;
            yield return null;
        }
        isStart = true;
        isGameEnd = false;

        //Debug.Log("1");
        if (onGameStartCallback != null)
            onGameStartCallback();

        gameTime = maxGameTime;

        float levelTime = maxGameTime;

        //Debug.Log("ddd : " + readyTime + " / " + maxGameTime);


        while (isGameEnd == false && gameTime > 0)
        {

            if(isPause == false)
            {
                gameTime -= Time.deltaTime;
                levelTime -= Time.deltaTime;
                //Debug.Log("ddd : " + readyTime + " /..... ");
                if (levelTime > 0)
                {
                    
                    float data = levelTime / maxGameTime;
                    //Debug.Log("a : " + readyTime + "  b : "  + data);
                    if (data <= 0.2f)
                    {
                        //Debug.Log("레벨 3");
                        gameLevel = 3;
                    }
                    else if (data <= 0.7f)
                    {
                        gameLevel = 2;
                    }
                    else
                    {
                        gameLevel = 1;
                    }
                   
                }
                else
                {
                    //Debug.Log("레벨 3??");
                    gameLevel = 3;
                }
                

                if (_feeverTime <= 0)
                {
                    isFeever = false;
                    UIFeever.Instance.Hide();
                }
                else
                    _feeverTime -= Time.deltaTime;
            }

            yield return null;
        }

        if (bestScore < score)
        {
            bestScore = score;
            isBestScore = true;
        }
            


        if (onGameEndCallback != null)
            onGameEndCallback();
        isStart = false;


        GoogleLoginTest.Instance.ReportScore((long)bestScore);
        

        gameStartCoroutine = null;
        Debug.Log("게임 종료");

        
    }
    public string googleStoreURL;
    public void OnClickGoogleStore()
    {
        Application.OpenURL(googleStoreURL);
    }

}
