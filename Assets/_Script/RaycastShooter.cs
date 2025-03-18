using UnityEngine;
using UnityEngine.UI;
using Unity.Mathematics;
using StarterAssets;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using System;

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
    public Animator animator;
    public ParticleSystem flash;
    public ParticleSystem bullethit;
    public int damageout;

    public Vector3 savedHitPosition;
    private bool isShooting = false;
    private int frameCounter = 0;
    private Vector3 lastMousePosition;
    private int ammo = 10;
    
    public TotalScore_Script totalScore_Script;

    public BangDanScript bangDanScript;
    public event Action<string, string, string> OnObjectShot;
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
       // totalScore_Script.ScoreHeadSort(20);
        // _bodyScoreText.text = _scoreBody.ToString();
        //SocreManager();
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
        RaycastHit[] hits = Physics.RaycastAll(ray, raycastDistance);

        bool foundHead = false;
        bool foundBody = false;

        foreach (RaycastHit hit in hits)
        {
            Transform hitTransform = hit.collider.transform;

            if (hitTransform.CompareTag("ZombieHead"))
            {
                foundHead = true;
            }
            else if (hitTransform.CompareTag("Zombie") || hitTransform.CompareTag("ZombieBody"))
            {
                foundBody = true;
            }
        }
        if (foundHead)
        {
            crosshair.color = hitZombieHeadColor;
        }
        else if (foundBody)
        {
            crosshair.color = hitZombieColor;
        }
        else
        {
            crosshair.color = defaultColor;
        }
    }



    public void HandleShooting()
    {
        //bangDan_SCRIPT.TruDan(1);
        if (Input.GetMouseButtonDown(0) && !bangDanScript.EmptyBullet)
        {
       
            isShooting = true;
            frameCounter = 0;

            bangDanScript.Shoot();
            //bangDan_SCRIPT.TruDan(1);

            flash.Play();
            Fire();
        }

        if (Input.GetMouseButton(0) && isShooting)
        {
            
            frameCounter++;
            if (frameCounter >= 2 || HasMouseMoved())
            {
                //bangDan_SCRIPT.TruDan(1);
                if (!flash.isPlaying)
                {
                flash.Play();
                bangDanScript.Shoot();
                Fire();

                frameCounter = 0;
                }
                
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
        animator.Play("Shoot", 0, 0);

        Vector3 direction = (savedHitPosition - firePoint.position).normalized;
        RaycastHit[] hits = Physics.RaycastAll(firePoint.position, direction, raycastDistance);

        bool isHeadshot = false;
        bool isBodyshot = false;
        RaycastHit? closestHit = null;
        float closestDistance = float.MaxValue;
        Heath targetHealth = null;

        foreach (RaycastHit hit in hits)
        {
            float distance = Vector3.Distance(firePoint.position, hit.point);

            if (distance < closestDistance)
            {
                closestHit = hit;
                closestDistance = distance;
            }

            Transform hitTransform = hit.collider.transform;

            if (hitTransform.CompareTag("ZombieHead"))
            {
                isHeadshot = true;
                targetHealth = hit.collider.GetComponent<Heath>();
            }
            else if (hitTransform.CompareTag("Zombie") || hitTransform.CompareTag("ZombieBody"))
            {
                isBodyshot = true;
                if (targetHealth == null)
                    targetHealth = hit.collider.GetComponent<Heath>();
            }

            if (hitTransform.parent != null)
            {
                Transform parent = hitTransform.parent;
                if (parent.CompareTag("ZombieHead"))
                {
                    isHeadshot = true;
                    targetHealth = parent.GetComponent<Heath>();
                }
                else if (parent.CompareTag("Zombie"))
                {
                    isBodyshot = true;
                    if (targetHealth == null)
                        targetHealth = parent.GetComponent<Heath>();
                }
            }
        }

        if (closestHit.HasValue && targetHealth != null)
        {
            RaycastHit hit = closestHit.Value;
            Instantiate(bullethit, hit.point, quaternion.identity);

            int damageToApply = damageout;
            string hitType = "";
            if (isHeadshot)
            {
                damageToApply = 10000;
                totalScore_Script.ScoreHeadSort(1);
                hitType = "Headshot";
                Debug.Log(hitType + " Damage: " + damageToApply);
            }
            else if (isBodyshot)
            {
                totalScore_Script.ScoreBody(1);
                hitType = "Bodyshot";
                Debug.Log(hitType + " Damage: " + damageToApply);
            }

            targetHealth.TakeDamage(damageToApply);

            string hitObjectName = hit.collider.name;
            string gunName = gameObject.name;
            OnObjectShot?.Invoke(hitType, hitObjectName, gunName);
        }
    }
    public void OnReloadComplete()
    {
        bangDanScript.OnReloadComplete();
    
    }

}
