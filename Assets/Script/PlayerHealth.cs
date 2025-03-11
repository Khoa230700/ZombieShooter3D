using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class PlayerHealth : MonoBehaviour
{
    public int Health;
    private int currentHealth;
    public Slider healthSlider;

    
    void Start()
    {
        currentHealth = Health;
        healthSlider.value = currentHealth;
    }
    private void Update()
    {
        healthSlider.value = currentHealth;
    }

    // Update is called once per frame
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthSlider.value = currentHealth;
        if (currentHealth <= 0)
        {
            Debug.Log("Die");
        }
    }
   
}
