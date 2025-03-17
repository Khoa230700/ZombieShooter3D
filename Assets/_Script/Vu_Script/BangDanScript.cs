using UnityEngine;
using UnityEngine.UI;

public class BangDanScript : MonoBehaviour
{
    [Header("Thông tin đạn")]
    public int _danDaNap;        // Đạn hiện tại trong băng đạn
    public int _danChuaNap;      // Đạn còn lại ngoài băng đạn
    public int _luudanDaNap;     // Giới hạn đạn trong một băng đạn
    public int _luudanChuaNap;   // Tổng số đạn tối đa có thể mang
    public int _soBangDan = 2;   // Số băng đạn còn lại

    [Header("UI & Animation")]
    public Text _soDanText;      // Hiển thị số lượng đạn
    public Animator animatorGun1;
    public Animator animatorGun2;

    private bool isReloading = false;
    public bool EmptyBullet;

    void Start()
    {
        UpdateAmmoDisplay();
        UpdateEmptyBulletStatus();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isReloading)
        {
            StartReload();
        }
    }

    public void Shoot()
    {
        if (_danDaNap > 0)
        {
            _danDaNap--;
            UpdateAmmoDisplay();
            UpdateEmptyBulletStatus();
        }
        else
        {
            Debug.Log("Hết đạn trong băng!");
            Reload();
        }
    }

    public void Reload()
    {
        if (_danDaNap < _luudanDaNap && _danChuaNap > 0)
        {
            int tinhDanThay = Mathf.Min(_luudanDaNap - _danDaNap, _danChuaNap);
            _danChuaNap -= tinhDanThay;
            _danDaNap += tinhDanThay;

            isReloading = true;
            PlayReloadAnimation();

            UpdateAmmoDisplay();
            UpdateEmptyBulletStatus();
        }
        else
        {
            Debug.Log("Không thể thay đạn: Đạn đã đầy hoặc hết đạn dự trữ!");
        }
    }

    public void StartReload()
    {
        if (!isReloading)
        {
            isReloading = true;
            PlayReloadAnimation();
            Reload();
        }
    }

    public void OnReloadComplete()
    {
        isReloading = false;
    }

    public void NhatBangDan()
    {
        if (_soBangDan > 0)
        {
            _danChuaNap = Mathf.Min(_danChuaNap + _luudanDaNap, _luudanChuaNap);
            _soBangDan--;
            UpdateAmmoDisplay();
            Debug.Log("Nhặt được băng đạn!");
        }
        else
        {
            Debug.Log("Không còn băng đạn để nhặt!");
        }
    }

    private void UpdateAmmoDisplay()
    {
        _soDanText.text = $"{_danDaNap} / {_danChuaNap}";
    }

    private void UpdateEmptyBulletStatus()
    {
        EmptyBullet = (_danDaNap == 0);
    }

    private void PlayReloadAnimation()
    {
        animatorGun1?.Play("Reload", 0, 0);
        animatorGun2?.Play("Reload", 0, 0);
    }
}
