using JetBrains.Annotations;
using System.Collections;
using System.Linq.Expressions;
//using TreeEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;
using static UnityEngine.GraphicsBuffer;

public class Zombie : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform BOT;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject Player;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] public int Damege = 40;
    [SerializeField] private CapsuleCollider capsuleCollider;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private AudioClip adidoclip;
    [SerializeField] private AudioSource adidocsource;

    public float attackCooldown = 3f;  // Thời gian chờ giữa các đợt tấn công
    private float nextAttackTime = 0f; // Thời điểm cho phép tấn công tiếp theo
    private int _speedHash;
    private int _attackHash;
    private void Awake()
    {
            if (Player == null)
            {
                Player = GameObject.FindGameObjectWithTag("Player");
            }
            if (playerHealth == null)
            {
            playerHealth = FindObjectOfType<PlayerHealth>();
        }

    }
    private void Start()
    {
        _speedHash = Animator.StringToHash("Run");
        //_attackHash = Animator.StringToHash("Attack");

    }
    private void Update()
    {
        
        agent.SetDestination(Player.transform.position);
        // tính khoảng cách giữa quái và vị trí ban đầu nhân vật
        var distance = Vector3.Distance(BOT.position, Player.transform.position);

        if (distance <= 2.2f)
        {
            animator.SetTrigger("Attack");
            adidocsource.Stop();
        }
        else
        {
            if (Vector3.Distance(transform.position, Player.transform.position) < 50f && !adidocsource.isPlaying)
            {
                adidocsource.PlayOneShot(adidoclip);
            //đuổi theo nhân vật
            }
            agent.SetDestination(Player.transform.position);
            animator.SetBool(_speedHash, true);
        }

        
    }
    public void Animationtrigger()
    {
        capsuleCollider.isTrigger = true;
    }
    public void animationEnd()
    {
        capsuleCollider.isTrigger = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("hit");
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(Damege);
            }
        }
    }




}
