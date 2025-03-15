using UnityEngine;
using UnityEngine.UI;

public class BangDanScript : MonoBehaviour
{

    public int _danDaNap ; // Đạn hiện tại trong băng đạn
    public int _danChuaNap ;
    public int _luudanDaNap; // Đạn hiện tại trong băng đạn
    public int _luudanChuaNap; // Số đạn tối đa trong băng đạn
    public int _soBangDan = 2;      // Số băng đạn còn lại

    public int tinhDanthay; // Đạn hiện tại trong băng đạn
    public int thayDan;

    public Text _soDanText;         // Text UI để hiển thị số đạn

    void Start()
    {
        UpdateAmmoDisplay();
    }

    public void UpdateAmmoDisplay()
    {
        _soDanText.text = $"{_danDaNap} / {_danChuaNap}";

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) 
        {
            Reload();
        }
    }
    public void Shoot()
    {
        if (_danDaNap > 0)
        {
            _danDaNap--;
            UpdateAmmoDisplay();
        }
        else
        {
            Reload();
            Debug.Log("Hết đạn trong băng!");
        }
    }

    public void Reload()
    {
        if(_danDaNap < _luudanDaNap)
        {
            tinhDanthay = _luudanDaNap - _danDaNap;
            if( tinhDanthay >= _danChuaNap)
            {
                tinhDanthay = _danChuaNap;
            }
            _danChuaNap -= tinhDanthay;
            _danDaNap += tinhDanthay;

            UpdateAmmoDisplay();                            
        }
        else
        {
            Debug.Log("Đạn đã đầy");
        }
    }

    // Hàm nhặt băng đạn
    public void PickupMagazine()
    {
        _soBangDan++; // Tăng số băng đạn còn lại
        UpdateAmmoDisplay();
        Debug.Log("Nhặt được băng đạn! Băng đạn còn lại: " + _soBangDan);
    }
}


