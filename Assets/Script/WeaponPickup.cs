using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] GunSO gunSO;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ActiveGun active = other.GetComponentInChildren<ActiveGun>();
            active.SwitchWeapon(gunSO);
            Destroy(this.gameObject);
        }
    }
}
