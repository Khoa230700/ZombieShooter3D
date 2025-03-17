using UnityEngine;

public class Heath : MonoBehaviour
{
    public int Health;
    private int currentHealth;
    private Spawn spawn;
    public Animator animator;
    private bool isDead = false;
    void Start()
    {
        spawn = FindFirstObjectByType<Spawn>();
        
        currentHealth = Health;
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        { 
            Die();
        }
    }
    public void Die()
    {
        if (isDead) return; // Nếu đã chết rồi thì không làm gì thêm
        isDead = true;
        if (spawn != null)
        {
            spawn.ZombieKilled();
        }
        animator.SetTrigger("DieFB");
        Destroy(gameObject, 2f);
    }
   
}
