using UnityEngine;
using UnityEngine.UI;

public class GunTypeController : MonoBehaviour
{
    [Header("Gun List")]
    public GameObject[] Weapons;

    [Header("Cross Hair List")]
    public RawImage[] CrossHairs;

    private int currentWeaponIndex = 0;

    void Start()
    {
        UpdateWeapon(0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            UpdateWeapon(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            UpdateWeapon(1);
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
}
