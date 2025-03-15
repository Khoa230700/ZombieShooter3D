using System.Collections.Generic;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InfoController : MonoBehaviour
{
    public Button Back;
    public Button RankingList;
    public Text WelcomeText;

    public Text RankText;
    public Text PointText;
    public Text WaveText;
    public Text KillText;
    public Text HeadshotText;

    private FirebaseAuth auth;
    private DatabaseReference databaseRef;

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        databaseRef = FirebaseDatabase.GetInstance(FirebaseApp.DefaultInstance, "https://zombieshooter-f4929-default-rtdb.asia-southeast1.firebasedatabase.app/").RootReference;

        FirebaseUser user = auth.CurrentUser;

        if (user != null)
        {
            WelcomeText.text = $"Welcome to the hell, {user.Email}";
            LoadPlayerData(user.UserId);
        }
        else
        {
            WelcomeText.text = "Welcome to the hell, guest";
        }

        Back.onClick.AddListener(BacktoSignin);
        RankingList.onClick.AddListener(ToRankingList);
    }


    void BacktoSignin()
    {
        SceneManager.LoadScene(0);
    }

    void ToRankingList()
    {
        SceneManager.LoadScene(2);
    }

    private void LoadPlayerData(string userId)
    {
        databaseRef.Child("users").Child(userId).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || !task.IsCompletedSuccessfully)
            {
                return;
            }

            if (task.Result.Exists)
            {
                DataSnapshot snapshot = task.Result;

                string rank = snapshot.Child("rank").Exists ? snapshot.Child("rank").Value.ToString() : "null";
                int point = snapshot.Child("point").Exists ? int.Parse(snapshot.Child("point").Value.ToString()) : 0;
                int wave = snapshot.Child("wave").Exists ? int.Parse(snapshot.Child("wave").Value.ToString()) : 0;
                int kill = snapshot.Child("kill").Exists ? int.Parse(snapshot.Child("kill").Value.ToString()) : 0;
                int headshot = snapshot.Child("headshot").Exists ? int.Parse(snapshot.Child("headshot").Value.ToString()) : 0;

                RankText.text = $"{rank}";
                PointText.text = $"{point}";
                WaveText.text = $"{wave}";
                KillText.text = $"{kill}";
                HeadshotText.text = $"{headshot}";

            }
        });
    }
}
