using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine.SceneManagement;
using Firebase.Auth;

public class LosePanelAppear : MonoBehaviour
{
    public GameObject losePanel;
    public GameObject PlayerObject;
    public PlayerHealth playerHealth;
    public Text killText_Lose;
    public Text headshotText_Lose;
    public Text totalPointText_Lose;
    public Text killText_HUD;
    public Text headshotText_HUD;
    public Text totalPointText_HUD;
    public Button bttOk;

    private DatabaseReference databaseRef;
    private string userId = "";

    void Start()
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        FirebaseUser user = auth.CurrentUser;

        if (user != null)
        {
            userId = user.UserId;
            string email = user.Email;
            Debug.Log($"Người chơi đã đăng nhập. Email: {email}, UserID: {userId}");
        }
        else
        {
            Debug.LogError("Không tìm thấy thông tin người dùng.");
            return;
        }

        databaseRef = FirebaseDatabase.DefaultInstance.RootReference;

        if (losePanel != null)
        {
            losePanel.SetActive(false);
        }

        bttOk.onClick.AddListener(ToWelcomeScene);
    }

    void Update()
    {
        if (playerHealth != null && playerHealth.currentHealth <= 0)
        {
            ShowLosePanel();
        }
    }

    void ShowLosePanel()
    {
        if (losePanel != null)
        {
            killText_Lose.text = killText_HUD.text;
            headshotText_Lose.text = headshotText_HUD.text;
            totalPointText_Lose.text = totalPointText_HUD.text;
            losePanel.SetActive(true);
            PlayerObject.SetActive(false);
            Time.timeScale = 0;

            StartCoroutine(CompareAndUpdatePlayerStats());
        }
    }

    void ToWelcomeScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Welcome");
    }

    IEnumerator CompareAndUpdatePlayerStats()
    {
        Debug.Log($"Truy vấn dữ liệu cho userId: {userId}");
        var task = databaseRef.Child("users").Child(userId).GetValueAsync();
        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            Debug.LogError("Lỗi khi truy vấn Firebase: " + task.Exception);
            yield break;
        }

        if (task.Result == null || !task.Result.Exists)
        {
            Debug.LogError($"Không tìm thấy dữ liệu cho userId: {userId}");
            yield break;
        }

        DataSnapshot snapshot = task.Result;

        int currentKill = SafeParseInt(snapshot.Child("kill").Value);
        int currentHeadshot = SafeParseInt(snapshot.Child("headshot").Value);
        int currentPoint = SafeParseInt(snapshot.Child("point").Value);
        int newKill = SafeParseInt(killText_Lose.text);
        int newHeadshot = SafeParseInt(headshotText_Lose.text);
        int newPoint = SafeParseInt(totalPointText_Lose.text);

        Debug.Log($"Dữ liệu cũ: kill={currentKill}, headshot={currentHeadshot}, point={currentPoint}");
        Debug.Log($"Dữ liệu mới: kill={newKill}, headshot={newHeadshot}, point={newPoint}");

        if (newKill > currentKill)
        {
            databaseRef.Child("users").Child(userId).Child("kill").SetValueAsync(newKill)
                .ContinueWithOnMainThread(t => Debug.Log("Kill updated in Firebase."));
        }
        if (newHeadshot > currentHeadshot)
        {
            databaseRef.Child("users").Child(userId).Child("headshot").SetValueAsync(newHeadshot)
                .ContinueWithOnMainThread(t => Debug.Log("Headshot updated in Firebase."));
        }
        if (newPoint > currentPoint)
        {
            databaseRef.Child("users").Child(userId).Child("point").SetValueAsync(newPoint)
                .ContinueWithOnMainThread(t => Debug.Log("Point updated in Firebase."));
        }
    }

    private int SafeParseInt(object value)
    {
        if (value == null) return 0;
        return int.TryParse(value.ToString(), out int result) ? result : 0;
    }
}
