using UnityEngine;
using StarterAssets;
using Unity.Mathematics;

public class Weapon : MonoBehaviour
{
    [Header("Weapon Settings")]
    public StarterAssetsInputs inputs;
    public AudioClip aduclip;
    public AudioSource adusource;
    public Animator animator;
    public ParticleSystem flash;
    public ParticleSystem bullethit;
    public GameObject hitvfx;
    public int damageout;
    public RaycastShooter shooter;
    [Header("Raycast Settings")]
    public Transform firePoint;
    public Camera mainCamera;
    public float raycastDistance = 100f;

    public void Shoot()
    {
        if (inputs.shoot)
        {
            Fire();
            inputs.ShootInput(false);
        }
    }

    void Fire()
    {
        if (firePoint == null || mainCamera == null) return;

        flash.Play();
        adusource.PlayOneShot(aduclip);
        animator.Play("Shoot", 0, 0);
        Vector3 targetPoint = shooter.savedHitPosition;

        if (Physics.Raycast(firePoint.position, (targetPoint - firePoint.position).normalized, out RaycastHit hit, raycastDistance))
        {
            Instantiate(bullethit, hit.point, quaternion.identity);
            Heath health = hit.collider.GetComponent<Heath>();
            if (health)
            {
                health.TakeDamage(damageout);
            }
        }
    }
}
