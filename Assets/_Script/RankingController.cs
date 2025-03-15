using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.UI;

public class RankingController : MonoBehaviour
{
    [Header("UI Components")]
    public GameObject[] rankingPanels;

    private DatabaseReference databaseRef;
    private bool firebaseInitialized = false;

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                databaseRef = FirebaseDatabase.DefaultInstance.RootReference;
                firebaseInitialized = true;
                LoadTopPlayers();
                StartCoroutine(UpdateRankingLoop());
            }
        });
    }

    void LoadTopPlayers()
    {
        if (!firebaseInitialized)
        {
            return;
        }

        databaseRef.Child("users").OrderByChild("point").LimitToLast(10).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                return;
            }

            if (!task.Result.Exists)
            {
                return;
            }

            List<(string userId, string email, int point, int kill, int headshot)> playerList = new List<(string, string, int, int, int)>();

            foreach (var child in task.Result.Children)
            {
                string userId = child.Key;
                string email = child.Child("email").Exists ? child.Child("email").Value.ToString() : "Unknown";
                int point = child.Child("point").Exists ? SafeParseInt(child.Child("point").Value) : 0;
                int kill = child.Child("kill").Exists ? SafeParseInt(child.Child("kill").Value) : 0;
                int headshot = child.Child("headshot").Exists ? SafeParseInt(child.Child("headshot").Value) : 0;

                playerList.Add((userId, email, point, kill, headshot));
            }
            playerList.Sort((a, b) => b.point.CompareTo(a.point));
            UpdateRankingUI(playerList);
        });
    }

    void UpdateRankingUI(List<(string userId, string email, int point, int kill, int headshot)> playerList)
    {
        for (int i = 0; i < rankingPanels.Length; i++)
        {
            if (i < playerList.Count)
            {
                var player = playerList[i];
                int rank = i + 1;
                Text[] texts = rankingPanels[i].GetComponentsInChildren<Text>();

                if (texts.Length >= 5)
                {
                    texts[0].text = rank.ToString(); 
                    texts[1].text = player.email; 
                    texts[2].text = player.point.ToString();
                    texts[3].text = player.kill.ToString();
                    texts[4].text = player.headshot.ToString();
                }
                rankingPanels[i].SetActive(true);
                databaseRef.Child("users").Child(player.userId).Child("rank").SetValueAsync(rank);
            }
            else
            {
                rankingPanels[i].SetActive(false);
            }
        }
    }
    IEnumerator UpdateRankingLoop()
    {
        while (true)
        {
            if (firebaseInitialized)
            {
                LoadTopPlayers();
            }
            yield return new WaitForSeconds(3f);
        }
    }

    private int SafeParseInt(object value)
    {
        if (value == null) return 0;
        int result;
        return int.TryParse(value.ToString(), out result) ? result : 0;
    }
}
