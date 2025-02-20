using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform Player;
    [SerializeField] private NavMeshAgent agent;
    private void Update()
    {
        agent.SetDestination(Player.position);
    }

}
