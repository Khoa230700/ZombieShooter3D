using UnityEngine;

public class GunController : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;

    private Vector3 targetPosition;

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
