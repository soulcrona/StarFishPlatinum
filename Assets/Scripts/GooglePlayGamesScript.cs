using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class GooglePlayGamesScript : MonoBehaviour
{
    //logs into the google play platform
    void Start()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();
        SignIn();
    }

    void SignIn()
    {
        Social.localUser.Authenticate(success => { });
    }

    #region Achievements
    public static void UnlockAchievements(string id)
    {
        Social.ReportProgress(id, 100, success => { });
    }

    public static void IncrementAchievements(string id, int stepsToIncrement)
    {
        PlayGamesPlatform.Instance.IncrementAchievement(id,stepsToIncrement, success => {});
    }
    #endregion

    public static void AddScoreToLeaderboard(string leaderboardID, long score)
    {
        Social.ReportScore(score, leaderboardID, success => { });
    }

    public static void ShowLeaderBoardUI()
    {
        Social.ShowLeaderboardUI();
    }
}
