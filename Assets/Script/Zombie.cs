using System.Collections;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class Zombie : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform BOT;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform Player;
    [SerializeField] private NavMeshAgent agent;
    
    private Vector3 _initPosition;



    private int _speedHash;
    private int _attackHash;
    private void Start()
    {
        _speedHash = Animator.StringToHash("Run");
        //_attackHash = Animator.StringToHash("Attack");

    }
    private void Update()
    {
        _initPosition = transform.position;
        agent.SetDestination(Player.position);
        // tính khoảng cách giữa quái và vị trí ban đầu nhân vật
        var distance = Vector3.Distance(BOT.position, Player.position);
        
        if (distance <=2)
        {
            animator.SetTrigger("Attack");
        }
        else
        {
            //đuổi theo nhân vật
            agent.SetDestination(Player.position);
            animator.SetBool(_speedHash,true);
        }

       
    }
       
   

}
