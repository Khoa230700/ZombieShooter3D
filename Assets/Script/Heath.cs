using UnityEngine;

public class Heath : MonoBehaviour
{
    public int enemyHealth;
    private int currentHealth;
    public Spawn spawn;
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
            if (spawn != null)
            {
                spawn.zombiesRemaining--;
                spawn.ZombieKilled();
            }
                Destroy(gameObject);
        }
    }
}
