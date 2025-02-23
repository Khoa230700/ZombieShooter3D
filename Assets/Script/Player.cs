using UnityEngine;
using UnityEngine.InputSystem.XInput;

public class Player : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float speed;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Spawn spawn;

    
    // Update is called once per frame
    void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        var move = transform.right * horizontal + transform.forward * vertical;
        characterController.Move(move * speed * Time.deltaTime);
       
       
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Zombie"))
        {
            Destroy(other.gameObject);
            Debug.Log("trung");
            spawn.zombiesRemaining--;
            spawn.ZombieKilled();
        }
    }
}
