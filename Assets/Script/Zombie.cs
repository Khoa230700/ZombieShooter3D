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
    [Header("Chỉ số của zombies")]
    public int Damage = 40;
    public int Health;
    public float Speed;
    public float attackCooldown = 3f;  // Thời gian chờ giữa các đợt tấn công

    [Header("Các chỉ số khác của zombies")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform BOT;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject Player;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private CapsuleCollider capsuleCollider;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private AudioClip adidoclip;
    [SerializeField] private AudioSource adidocsource;
    
    private float nextAttackTime = 0f; // Thời điểm cho phép tấn công tiếp theo
    private int _speedHash;
    private int _attackHash;
    public Heath ZombieHealth;

    private void Awake()
    {
            if (Player == null)
            {
                Player = GameObject.FindGameObjectWithTag("Player");
            }
            if (playerHealth == null)
            {
            playerHealth = FindFirstObjectByType<PlayerHealth>(); 
            }

    }
    private void Start()
    {
        ZombieHealth.Health = Health;
        _speedHash = Animator.StringToHash("Run");
        //_attackHash = Animator.StringToHash("Attack");
        agent.speed = Speed;

    }
    private void Update()
    {
        ZombieHealth.Health = Health;
        agent.speed = Speed;
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
                playerHealth.TakeDamage(Damage);
            }
        }
    }




}
