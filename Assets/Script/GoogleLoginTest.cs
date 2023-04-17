using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class GoogleLoginTest : MonoBehaviour
{
    public static GoogleLoginTest Instance;
    private void Awake()
    {
        Instance = this;
    }

    
    //public Text text_Test;
    void Start()
    {
        // Create client configuration
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();

        // Enable debugging output (recommended)
        PlayGamesPlatform.DebugLogEnabled = true;

        // Initialize and activate the platform
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();
        SignIn();
    }
    
    // private void ProcessAuthentication(SignInStatus status) 
    // {
    //     Debug.Log($"ProcessAuthentication {status}");
    //     if (status == SignInStatus.Success)
    //     {
    //         //디버깅에 권장됨
    //         PlayGamesPlatform.DebugLogEnabled = true;
    //         //PlayGamesPlatform 활성화
    //         PlayGamesPlatform.Activate();
    //         SignIn();
    //     } 
    //     else 
    //     {
    //         // Disable your integration with Play Games Services or show a login button
    //         // to ask users to sign-in. Clicking it should call
    //         // PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication).
    //     }
    // }

    public void SignIn()
    {
#if UNITY_ANDROID
        //text_Test.text = "로그인 시도";
        Social.localUser.Authenticate((bool success) =>
        {
            if (success)
            {
                Debug.Log("Login Success!!!!");
                //text_Test.text = "로그인 됨";
                // to do ...
                // 구글 플레이 게임 서비스 로그인 성공 처리
            }
            else
            {
                Debug.Log("Login Failed!!!!");
                //text_Test.text = "로그인 실패";
                // to do ...
                // 구글 플레이 게임 서비스 로그인 실패 처리
            }
        });

#elif UNITY_IOS
 
        Social.localUser.Authenticate((bool success) =>
        {
            if (success)
            {
                // to do ...
                // 애플 게임 센터 로그인 성공 처리
            }
            else
            {
                // to do ...
                // 애플 게임 센터 로그인 실패 처리
            }
        });
 
#endif
    }

    public void UnlockAchievement(int score)
    {
        if (score >= 15)
        {
            //PlayGamesPlatform.Instance.ReportProgress(GPGSIds.achievement_test3, 15f, null);
        }
        else if (score >= 10)
        {
            //PlayGamesPlatform.Instance.ReportProgress(GPGSIds.achievement_test2, 10f, null);
        }
        else if (score >= 5)
        {
#if UNITY_ANDROID
            PlayGamesPlatform.Instance.ReportProgress(GPGSIds.achievement_feever_x_5, 5f, null);
#elif UNITY_IOS
            Social.ReportProgress("Score_100", 100f, null);
#endif
        }

    }


    public void ShowAchievementUI()
    {
        // Sign In 이 되어있지 않은 상태라면
        // Sign In 후 업적 UI 표시 요청할 것
        if (Social.localUser.authenticated == false)
        {
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    // Sign In 성공
                    // 바로 업적 UI 표시 요청
                    Social.ShowAchievementsUI();
                    return;
                }
                else
                {
                    // Sign In 실패 처리
                    return;
                }
            });
        }

        Social.ShowAchievementsUI();
    }

    public void ReportScore(long score)
    {
#if UNITY_ANDROID

        PlayGamesPlatform.Instance.ReportScore(score, GPGSIds.leaderboard_best_score, (bool success) =>
        {
            if (success)
            {
                // Report 성공
                // 그에 따른 처리
            }
            else
            {
                // Report 실패
                // 그에 따른 처리
            }
        });

#elif UNITY_IOS
 
        Social.ReportScore(score, "Leaderboard_ID", (bool success) =>
            {
                if (success)
                {
                    // Report 성공
                    // 그에 따른 처리
                }
                else
                {
                    // Report 실패
                    // 그에 따른 처리
                }
            });
        
#endif
    }

    public void ShowLeaderboardUI()
    {
        // Sign In 이 되어있지 않은 상태라면
        // Sign In 후 리더보드 UI 표시 요청할 것
        if (Social.localUser.authenticated == false)
        {
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    // Sign In 성공
                    // 바로 리더보드 UI 표시 요청
                    Social.ShowLeaderboardUI();
                    return;
                }
                else
                {
                    // Sign In 실패 
                    // 그에 따른 처리
                    return;
                }
            });
        }

#if UNITY_ANDROID
        PlayGamesPlatform.Instance.ShowLeaderboardUI();
#elif UNITY_IOS
        GameCenterPlatform.ShowLeaderboardUI("Leaderboard_ID", UnityEngine.SocialPlatforms.TimeScope.AllTime);
#endif
    }
    int score;
    public void OnClickScore()
    {
        score++;
        //text_Test.text = score.ToString();
        UnlockAchievement(score);
        ReportScore(score);
    }
}
