using UnityEditor;
using UnityEngine;
[System.Serializable]
public class ActiveGun : MonoBehaviour
{
    public GunSO gunso;
    public Animator animator;
    [SerializeField] public RaycastShooter currentshooter;
    public AudioClip aduclip;
    public AudioSource adusource;
    //public ParticleSystem flash;
    //public ParticleSystem bullethit;
    float timelastshot;
    void Start()
    {
        currentshooter = FindObjectOfType<RaycastShooter>();
    }

    // Update is called once per frame
    void Update()
    {
        timelastshot += Time.deltaTime;
        HandleShoot();
    }
    public void SwitchWeapon(GunSO gunso)
    {
        Debug.Log("pickup" + gunso.name);
    }
    public void HandleShoot()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)) // Nếu người chơi bấm chuột trái
        {
            if (timelastshot >= gunso.FireRate)
            {
                //currentshooter.CheckMouseClick(gunso);
                //flash.Play();
                adusource.PlayOneShot(aduclip);
                animator.Play("RilfeShoot", 0, 0);
                timelastshot = 0f;

            }
        }
    }
}
