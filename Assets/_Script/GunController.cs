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
                frameCounter = 0;
            }
            if (Input.GetMouseButtonUp(0))
            {
                isShooting = false;
            }

            if (isShooting)
            {
                frameCounter++;
                if (frameCounter >= autoFireRateFrames)
                {
                    frameCounter = 0;
                    Fire();
                    if (ShootPoint != null)
                    {
                        ShootPoint.HandleShooting();
                    }
                    if (gunRecoil != null)
                    {
                        gunRecoil.ApplyRecoil();
                    }
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                Fire();
                if(ShootPoint != null)
                {
                    ShootPoint.HandleShooting();
                }    
                if (gunRecoil != null)
                {
                    gunRecoil.ApplyRecoil();
                }
            }
        }
    }

    private void Fire()
    {
        SetTargetPosition(ShootPoint.savedHitPosition);
        Debug.Log($"Firing at target position: {targetPosition}"); // Kiểm tra tọa độ
        FireBullet();
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
