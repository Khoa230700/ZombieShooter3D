using UnityEngine;

public class Heath : MonoBehaviour
{
    public int enemyHealth;
    private int currentHealth;
    public Spawn spawn;
    public Animator animator;
    private bool isDead = false;
    void Start()
    {
        spawn = FindObjectOfType<Spawn>();
        
        currentHealth = enemyHealth;
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
            spawn.zombiesRemaining--;
            spawn.ZombieKilled();
        }
        animator.SetTrigger("DieFB");
        Destroy(gameObject, 2f);
    }
}
