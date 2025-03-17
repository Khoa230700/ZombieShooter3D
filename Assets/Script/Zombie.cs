using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    [Header("Chỉ số của zombies")]
    public int Damage { get; private set; }
    public int Health { get; private set; }
    public float Speed { get; private set; }
    public float AttackCooldown = 3f;

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

    private float nextAttackTime = 0f;
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

    public void Initialize(int damage, int health, float speed)
    {
        Damage = damage;
        Health = health;
        Speed = speed;

        if (ZombieHealth != null)
        {
            ZombieHealth.Health = Health;
        }

        if (agent != null)
        {
            agent.speed = Speed;
        }

        Debug.Log($"Zombie Spawned: Damage={Damage}, Health={Health}, Speed={Speed}");
    }

    private void Start()
    {
        _speedHash = Animator.StringToHash("Run");
        _attackHash = Animator.StringToHash("Attack");
    }

    private void Update()
    {
        if (ZombieHealth != null)
        {
            ZombieHealth.Health = Health;
        }

        agent.speed = Speed;
        agent.SetDestination(Player.transform.position);

        var distance = Vector3.Distance(BOT.position, Player.transform.position);

        if (distance <= 2.2f)
        {
            if (Time.time >= nextAttackTime)
            {
                animator.SetTrigger(_attackHash);
                nextAttackTime = Time.time + AttackCooldown;
                adidocsource.Stop();
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, Player.transform.position) < 50f && !adidocsource.isPlaying)
            {
                adidocsource.PlayOneShot(adidoclip);
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
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(Damage);
            }
        }
    }
}
