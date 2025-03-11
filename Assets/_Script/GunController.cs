using UnityEngine;

public class GunController : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public RaycastShooter ShootPoint;
    public GunRecoil gunRecoil;

    [Header("Gun Settings")]
    public bool isAutoFire = false;
    public int autoFireRateFrames = 20;

    private Vector3 targetPosition;
    private int frameCounter = 0;
    private bool isShooting = false;
    private bool wasQuickTap = false;

    private void Update()
    {
        ShootType();
    }

    public void ShootType()
    {
        if (ShootPoint == null) return;

        if (isAutoFire)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isShooting = true;
                wasQuickTap = true;
                frameCounter = 0;
            }

            if (Input.GetMouseButton(0) && isShooting)
            {
                frameCounter++;
                if (frameCounter >= autoFireRateFrames)
                {
                    frameCounter = 0;
                    Fire();
                    wasQuickTap = false;
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                isShooting = false;
                if (wasQuickTap)
                {
                    Fire();
                    wasQuickTap = false;
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                Fire();
            }
        }
    }

    private void Fire()
    {
        SetTargetPosition(ShootPoint.savedHitPosition);
        //Debug.Log($"Firing at target position: {targetPosition}");
        FireBullet();
        if (ShootPoint != null)
        {
            ShootPoint.HandleShooting();
        }
        if (gunRecoil != null)
        {
            gunRecoil.ApplyRecoil();
        }
    }

    public void SetTargetPosition(Vector3 position)
    {
        targetPosition = position;
    }

    public void FireBullet()
    {
        if (bulletPrefab == null || firePoint == null) return;

        GameObject bulletObject = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Bullet bullet = bulletObject.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.Initialize(targetPosition);
        }
    }
}
