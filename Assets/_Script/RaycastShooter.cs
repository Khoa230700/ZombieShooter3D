using UnityEngine;
using UnityEngine.UI;

public class RaycastShooter : MonoBehaviour
{
    public Transform shootPoint;
    public Camera mainCamera;
    public RawImage crosshair;
    public float raycastDistance = 100f;
    public Color defaultColor = Color.white;
    public Color hitZombieColor = Color.red;
    public Color hitZombieHeadColor = Color.green;
    public Vector3 savedHitPosition;
    public GunRecoil gunRecoil;

    public GunController gunController; // Tham chiếu đến GunController

    void Start()
    {
        if (crosshair != null)
        {
            crosshair.color = defaultColor;
        }
    }

    void Update()
    {
        ShootRaycast();
        CheckMouseClick();
    }

    void ShootRaycast()
    {
        if (shootPoint == null || mainCamera == null) return;

        Vector3 mousePosition = Input.mousePosition;
        Ray cameraRay = mainCamera.ScreenPointToRay(mousePosition);
        RaycastHit cameraHit;

        Vector3 targetPoint = shootPoint.position + shootPoint.forward * raycastDistance;
        if (Physics.Raycast(cameraRay, out cameraHit, raycastDistance))
        {
            targetPoint = cameraHit.point;
        }

        Vector3 direction = (targetPoint - shootPoint.position).normalized;
        Ray ray = new Ray(shootPoint.position, direction);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            if (hit.collider.CompareTag("ZombieHead"))
            {
                crosshair.color = hitZombieHeadColor;
            }
            else if (hit.collider.CompareTag("Zombie") || hit.collider.CompareTag("ZombieBody"))
            {
                crosshair.color = hitZombieColor;
            }
            else
            {
                crosshair.color = defaultColor;
            }
        }
        else
        {
            crosshair.color = defaultColor;
        }
    }

    void CheckMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            Ray cameraRay = mainCamera.ScreenPointToRay(mousePosition);
            RaycastHit cameraHit;

            Vector3 targetPoint;
            float minDistance = 2f;

            if (Physics.Raycast(cameraRay, out cameraHit, raycastDistance))
            {
                float hitDistance = Vector3.Distance(cameraRay.origin, cameraHit.point);

                if (hitDistance < minDistance)
                {
                    targetPoint = cameraHit.point + cameraRay.direction * (minDistance - hitDistance);
                }
                else
                {
                    targetPoint = cameraHit.point;
                }
            }
            else
            {
                targetPoint = cameraRay.origin + cameraRay.direction * raycastDistance;
            }

            savedHitPosition = targetPoint;
            Debug.Log($"Shooting at: {savedHitPosition}");
            if (gunController != null)
            {
                gunController.SetTargetPosition(savedHitPosition);
                gunController.FireBullet();
                
            }
            if (gunRecoil != null)
            {
                gunRecoil.ApplyRecoil();
            }
        }
    }

}
