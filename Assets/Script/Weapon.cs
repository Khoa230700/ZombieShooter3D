using UnityEngine;
using StarterAssets;
using Unity.Mathematics;

public class Weapon : MonoBehaviour
{
    public StarterAssetsInputs inputs;
    public AudioClip aduclip;
    public AudioSource adusource;
    public Animator animator;
    public ParticleSystem flash;
    public Transform raycasthit;
    public ParticleSystem bullethit;
    public GameObject hitvfx;
    public float bulletspeed;
    public Transform bulletpoint;
    public int damageout;

    
    // Update is called once per frame
    void Update()
    {
        if (inputs.shoot)
        {
            flash.Play();
            adusource.PlayOneShot(aduclip);
            animator.Play("Shoot", 0, 0);
            RaycastHit hit;
            if (Physics.Raycast(raycasthit.transform.position, raycasthit.transform.forward
             , out hit, Mathf.Infinity))
            {
                Instantiate(bullethit,hit.point,quaternion.identity);
                Heath health = hit.collider.GetComponent<Heath>();
                if (health)
                {
                health.TakeDamage(damageout);
                }
            }
            inputs.ShootInput(false);
        }
    }
    
}
