using UnityEngine;
using UnityEngine.UI;
using Unity.Mathematics;
using StarterAssets;

public class RaycastShooter : MonoBehaviour
{
    [Header("Raycast Settings")]
    public Transform firePoint;
    public Transform shootPoint;
    public Camera mainCamera;
    public float raycastDistance = 100f;

    [Header("Crosshair Settings")]
    public RawImage crosshair;
    public Color defaultColor = Color.white;
    public Color hitZombieColor = Color.red;
    public Color hitZombieHeadColor = Color.green;

    [Header("Weapon Settings")]
    public StarterAssetsInputs inputs;
    public AudioClip aduclip;
    public AudioSource adusource;
    public Animator animator;
    public ParticleSystem flash;
    public ParticleSystem bullethit;
    public int damageout;

    public Vector3 savedHitPosition;
    private bool isShooting = false;
    private int frameCounter = 0;
    private Vector3 lastMousePosition;

    public BangDan_sCRIPT bangDan_SCRIPT;
    void Start()
    {
        if (crosshair != null)
        {
            crosshair.color = defaultColor;
        }
    }

    void Update()
    {
        UpdateTargetPosition();
        HandleShooting();
    }

    void UpdateTargetPosition()
    {
        if (shootPoint == null || mainCamera == null) return;

        Vector3 mousePosition = Input.mousePosition;
        Ray cameraRay = mainCamera.ScreenPointToRay(mousePosition);
        RaycastHit cameraHit;

        Vector3 targetPoint = cameraRay.origin + cameraRay.direction * raycastDistance;

        if (Physics.Raycast(cameraRay, out cameraHit, raycastDistance))
        {
            targetPoint = cameraHit.point;
        }

        // Nếu quá gần vật thể, tránh thay đổi vị trí đột ngột
        float minDistance = 2f;
        float hitDistance = Vector3.Distance(shootPoint.position, targetPoint);

        if (hitDistance < minDistance)
        {
            targetPoint = shootPoint.position + cameraRay.direction * minDistance;
        }

        savedHitPosition = targetPoint;
        UpdateCrosshair(targetPoint);
    }

    void UpdateCrosshair(Vector3 targetPoint)
    {
        Ray ray = new Ray(shootPoint.position, (targetPoint - shootPoint.position).normalized);
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

    public void HandleShooting()
    {
        //bangDan_SCRIPT.TruDan(1);
        if (Input.GetMouseButtonDown(0))
        {
            isShooting = true;
            frameCounter = 0;
            bangDan_SCRIPT.TruDan(1);
            Fire();

        }

        if (Input.GetMouseButton(0) && isShooting)
        {
            frameCounter++;
            if (frameCounter >= 2 || HasMouseMoved())
            {
                bangDan_SCRIPT.TruDan(1);
                Fire();
                frameCounter = 0;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isShooting = false;
        }
    }

    bool HasMouseMoved()
    {
        if (Input.mousePosition != lastMousePosition)
        {
            lastMousePosition = Input.mousePosition;
            return true;
        }
        return false;
    }

    void Fire()
    {
        if (firePoint == null || mainCamera == null) return;

        flash.Play();
        adusource.PlayOneShot(aduclip);
        animator.Play("Shoot", 0, 0);

        Vector3 direction = (savedHitPosition - firePoint.position).normalized;
        if (Physics.Raycast(firePoint.position, direction, out RaycastHit hit, raycastDistance))
        {
            Instantiate(bullethit, hit.point, quaternion.identity);

            Heath health = hit.collider.GetComponent<Heath>();
            if (health)
            {
                Debug.Log($"Hit: {hit.collider.name}, Damage: {damageout}");
                health.TakeDamage(damageout);
            }
        }
        //bangDan_SCRIPT.TruDan(1);
    }
}
