using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GunTypeController : MonoBehaviour
{
    [Header("Gun List")]
    public GameObject[] Weapons;

    [Header("Cross Hair List")]
    public RawImage[] CrossHairs;

    private int currentWeaponIndex = 0;
    public Collectbullet collect;
    public BangDanScript piston;
    public BangDanScript machinegun;

    void Start()
    {
        UpdateWeapon(0);
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            UpdateWeapon(0);
            Reloadbullet(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            UpdateWeapon(1);
            Reloadbullet(1);
        }
    }

    void UpdateWeapon(int weaponIndex)
    {
        if (Weapons == null || CrossHairs == null) return;

        for (int i = 0; i < Weapons.Length; i++)
        {
            bool isActive = (i == weaponIndex);
            if (Weapons[i] != null) Weapons[i].SetActive(isActive);
            if (CrossHairs[i] != null) CrossHairs[i].gameObject.SetActive(isActive);
        }
        currentWeaponIndex = weaponIndex;
    }

    public void Reloadbullet(int weaponIndex)
    {
        if (weaponIndex == 0)
        {
            piston._danChuaNap += collect.Pistondanchuanap;
            collect.Pistondanchuanap = 0;
        }
        else if (weaponIndex == 1)
        {
            machinegun._danChuaNap += collect.Machinegundanchuanap;
            collect.Machinegundanchuanap = 0;
        }
    }
}
