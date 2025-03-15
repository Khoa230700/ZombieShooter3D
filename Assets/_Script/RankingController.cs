using System.Collections.Generic;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.UI;

public class RankingController : MonoBehaviour
{
    [Header("UI Components")]
    public Transform rankingContainer;  // Content của ScrollView
    public GameObject rankingPrefab;    // Prefab panel chứa thông tin người chơi

    private DatabaseReference databaseRef;
    private bool firebaseInitialized = false;

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                FirebaseApp app = FirebaseApp.DefaultInstance;

                // ✅ Kiểm tra và khởi tạo Firebase Database
                if (FirebaseDatabase.DefaultInstance == null)
                {
                    FirebaseDatabase.DefaultInstance.SetPersistenceEnabled(true);
                }

                databaseRef = FirebaseDatabase.DefaultInstance.RootReference;
                firebaseInitialized = true;
                LoadTopPlayers();
            }
            else
            {
                Debug.LogError("❌ Firebase dependencies are not available.");
            }
        });
    }

    void LoadTopPlayers()
    {
        if (!firebaseInitialized)
        {
            Debug.LogWarning("⚠️ Firebase chưa được khởi tạo!");
            return;
        }

        Debug.Log("📌 Bắt đầu truy vấn top 10 người chơi...");

        databaseRef.Child("users").OrderByChild("point").LimitToLast(10).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("❌ Lỗi khi lấy dữ liệu người chơi: " + task.Exception?.GetBaseException().Message);
                return;
            }

            if (!task.Result.Exists)
            {
                Debug.Log("📌 Không có người chơi nào trong database!");
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

                Debug.Log($"🔹 {email} - Điểm: {point}, Kill: {kill}, Headshot: {headshot}");

                playerList.Add((userId, email, point, kill, headshot));
            }

            // Sắp xếp danh sách theo point giảm dần
            playerList.Sort((a, b) => b.point.CompareTo(a.point));

            // Hiển thị danh sách xếp hạng lên UI
            UpdateRankingUI(playerList);
        });
    }

    void UpdateRankingUI(List<(string userId, string email, int point, int kill, int headshot)> playerList)
    {
        // Xóa tất cả item cũ trong rankingContainer
        foreach (Transform child in rankingContainer)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < playerList.Count; i++)
        {
            var player = playerList[i];
            int rank = i + 1;

            // Tạo item xếp hạng từ prefab
            GameObject newItem = Instantiate(rankingPrefab, rankingContainer);

            // Lấy các Text trong prefab
            newItem.transform.Find("Rank").GetComponent<Text>().text = rank.ToString();
            newItem.transform.Find("PlayerName").GetComponent<Text>().text = player.email;
            newItem.transform.Find("Point").GetComponent<Text>().text = player.point.ToString();
            newItem.transform.Find("Kill").GetComponent<Text>().text = player.kill.ToString();
            newItem.transform.Find("Headshot").GetComponent<Text>().text = player.headshot.ToString();

            // Cập nhật rank của người chơi vào Firebase
            databaseRef.Child("users").Child(player.userId).Child("rank").SetValueAsync(rank);
        }

        Debug.Log("✅ Bảng xếp hạng đã cập nhật thành công!");
    }

    private int SafeParseInt(object value)
    {
        if (value == null) return 0;
        int result;
        return int.TryParse(value.ToString(), out result) ? result : 0;
    }
}