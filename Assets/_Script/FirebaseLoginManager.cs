using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FirebaseLoginManager : MonoBehaviour
{
    [Header("Đăng ký")]
    public InputField RegisterEmail;
    public InputField Password;
    public InputField RePassword;
    public Button BttRegister;
    public Text Notification;

    private FirebaseAuth auth;
    private FirebaseDatabase database;
    private DatabaseReference reference;

    [Header("Đăng nhập")]
    public InputField LoginEmail;
    public InputField LoginPassword;
    public Text LoginNoti;
    public Button BttLogin;

    [Header("Turn On/Off Panel")]
    public GameObject PanelLogin;
    public GameObject PanelSignup;
    public Button BttSignup;
    public Button BttBack;
    private DatabaseReference databaseRef;
    private void Start()
    {
        databaseRef = FirebaseDatabase.DefaultInstance.RootReference;
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                FirebaseApp app = FirebaseApp.DefaultInstance;
                auth = FirebaseAuth.DefaultInstance;
                database = FirebaseDatabase.GetInstance(app, "https://zombieshooter-f4929-default-rtdb.asia-southeast1.firebasedatabase.app/");
                reference = database.RootReference;

            }
        });

        BttRegister.onClick.AddListener(RegisterFirebase);
        BttLogin.onClick.AddListener(LoginFirebase);
        BttBack.onClick.AddListener(Login);
        BttSignup.onClick.AddListener(SignUp);
        Login();
    }

    private void RegisterFirebase()
    {
        string email = RegisterEmail.text.Trim();
        string password = Password.text.Trim();
        string confirmPassword = RePassword.text.Trim();

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            Notification.text = "Email hoặc mật khẩu không được để trống!";
            return;
        }

        if (password.Length < 6)
        {
            Notification.text = "Mật khẩu phải có ít nhất 6 ký tự!";
            return;
        }

        if (password != confirmPassword)
        {
            Notification.text = "Mật khẩu không khớp!";
            return;
        }

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Notification.text = $"Lỗi: {task.Exception?.GetBaseException().Message}";
                return;
            }

            if (task.IsCompletedSuccessfully)
            {
                FirebaseUser newUser = task.Result.User;
                if (newUser != null)
                {
                    Notification.text = "Đăng ký thành công!";
                    StartCoroutine(SaveUserDataWithDelay(newUser));
                }
            }
        });
    }

    public void LoginFirebase()
    {
        string email = LoginEmail.text.Trim();
        string password = LoginPassword.text.Trim();

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            LoginNoti.text = "Email hoặc mật khẩu không được để trống!";
            return;
        }

        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                LoginNoti.text = "Đăng nhập bị hủy!";
                return;
            }
            if (task.IsFaulted)
            {
                FirebaseException firebaseEx = task.Exception?.GetBaseException() as FirebaseException;
                if (firebaseEx != null)
                {
                    LoginNoti.text = $"Lỗi đăng nhập: {firebaseEx.Message}";
                }
                else
                {
                    LoginNoti.text = "Đăng nhập thất bại. Vui lòng kiểm tra email/mật khẩu!";
                }
                return;
            }
            if (task.IsCompletedSuccessfully)
            {
                FirebaseUser user = task.Result.User;
                EnsureUserDataExists(user);
                StartCoroutine(DelayedSceneLoad(1));
            }
        });
    }

    IEnumerator DelayedSceneLoad(int sceneIndex)
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneIndex);
    }

    private void EnsureUserDataExists(FirebaseUser user)
    {
        if (user == null) return;

        databaseRef.Child("users").Child(user.UserId).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (!task.Result.Exists)
            {
                Dictionary<string, object> userData = new Dictionary<string, object>
            {
                { "email", user.Email },
                { "rank", "null" },
                { "point", 0 },
                { "wave", 0 },
                { "kill", 0 },
                { "headshot", 0 }
            };

                databaseRef.Child("users").Child(user.UserId).SetValueAsync(userData).ContinueWithOnMainThread(saveTask =>
                {
                    if (saveTask.IsCompletedSuccessfully)
                    {
                    }
                });
            }
        });
    }


    private void SaveUserData(FirebaseUser user)
    {

        if (string.IsNullOrEmpty(user.UserId))
        {
            return;
        }

        if (reference == null)
        {
            return;
        }

        Dictionary<string, object> userData = new Dictionary<string, object>
        {
            { "email", user.Email },
            { "rank", "null" },
            { "point", 0 },
            { "wave", 0 },
            { "kill", 0 },
            { "headshot", 0 }
        };


        reference.Child("users").Child(user.UserId).SetValueAsync(userData).ContinueWithOnMainThread(task =>
        {
        });
    }

    private IEnumerator SaveUserDataWithDelay(FirebaseUser user)
    {
        yield return new WaitForSeconds(2f);
        SaveUserData(user);
    }

    public void SignUp()
    {
        PanelLogin.SetActive(false);
        PanelSignup.SetActive(true);
        LoginPassword.text = "";
    }

    public void Login()
    {
        PanelSignup.SetActive(false);
        PanelLogin.SetActive(true);
        LoginPassword.text = "";
    }
}
