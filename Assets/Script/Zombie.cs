using System.Collections;
using System.Linq.Expressions;
using TreeEditor;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class Zombie : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform BOT;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject Player;
    [SerializeField] private NavMeshAgent agent;
     
    private int _speedHash;
    private int _attackHash;
    private void Awake()
    {
            if (Player == null)
            {
                Player = GameObject.FindGameObjectWithTag("Player");
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
        
        if (distance <=2)
        {
            animator.SetTrigger("Attack");
        }
        else
        {
            //đuổi theo nhân vật
            agent.SetDestination(Player.transform.position);
            animator.SetBool(_speedHash,true);
        }

       
    }
       
   

}
