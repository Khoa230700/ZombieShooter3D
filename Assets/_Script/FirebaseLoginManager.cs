using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.Rendering;
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


    private void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        BttRegister.onClick.AddListener(RegisterFirebase);
        BttLogin.onClick.AddListener(LoginFirebase);
        BttBack.onClick.AddListener(Login);
        BttSignup.onClick.AddListener(SignUp);
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
                FirebaseException firebaseEx = task.Exception?.GetBaseException() as FirebaseException;
                if (firebaseEx != null)
                {
                    Notification.text = $"Lỗi: {firebaseEx.Message}";
                }
                else
                {
                    Notification.text = "Đăng ký thất bại. Vui lòng thử lại!";
                }
                return;
            }

            if (task.IsCompletedSuccessfully)
            {
                Notification.text = "Đăng ký thành công!";
            }
        });
    }

    public void LoginFirebase()
    {
        string email = LoginEmail.text;
        string password = Password.text;

        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if(task.IsCanceled)
            {
                LoginNoti.text = "Login Canceled";
            }    
            if(task.IsFaulted)
            {
                LoginNoti.text = "Fail to login!!";
            }    
            if(task.IsCompleted)
            {
                LoginNoti.text = "Login Success!!";
                FirebaseUser user = task.Result.User;

                SceneManager.LoadScene("RankingScene");
            }    

        });
    }    

    public void SignUp()
    {
        PanelLogin.SetActive(false);
        PanelSignup.SetActive(true);
    }

    public void Login()
    {
        PanelSignup.SetActive(false);
        PanelLogin.SetActive(true);
    }
}
