using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class PlayerHealth : MonoBehaviour
{
    private int Health;
    public int currentHealth;
    public Slider healthSlider;
    private int Amor;
    public int currentArmor;
    public Slider AmorSlider;
    void Start()
    {
        Health = 100;
        Amor = 100;
        currentHealth = Health;
        healthSlider.value = currentHealth;
        currentArmor = Amor;
        AmorSlider.value = currentArmor;
    }
    private void Update()
    {
        healthSlider.value = currentHealth;

        AmorSlider.value = currentArmor;
    }
    public void TakeDamage(int damage)
    {
        int armorDamage = Mathf.FloorToInt(damage * 0.7f);
        int healthDamage = Mathf.FloorToInt(damage * 0.3f);

        if (currentArmor > 0)
        {
            if (currentArmor >= armorDamage)
            {
                currentArmor -= armorDamage;
            }
            else
            {
                int remainingDamage = armorDamage - currentArmor;
                currentArmor = 0;
                currentHealth -= remainingDamage;
            }
        }
        else
        {
            currentHealth -= damage;
        }
        currentHealth -= healthDamage;
        if (currentArmor < 0) currentArmor = 0;
        if (currentHealth < 0) currentHealth = 0;

        Debug.Log($"Armor: {currentArmor} - Health: {currentHealth}");
    }
}
