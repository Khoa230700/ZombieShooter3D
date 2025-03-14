using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class PlayerHealth : MonoBehaviour
{
    public int Health;
    private int currentHealth;
    public Slider healthSlider;
    public int Amor;
    private int currentArmor;
    public Slider AmorSlider;
    void Start()
    {
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

    // Update is called once per frame
    //public void TakeDamage(int damage)
    //{
    //    currentHealth -= damage;
    //    healthSlider.value = currentHealth;
    //    if (currentHealth <= 0)
    //    {
    //        Debug.Log("Die");
    //    }
    //}
    public void TakeDamage(int damage)
    {
        if (currentArmor > 0)
        {
            // Trừ damage vào giáp
            currentArmor -= damage;

            // Tính damage giảm cho máu (khi còn giáp, máu chỉ nhận damage theo hệ số 0.3)
            int reducedDamage = Mathf.FloorToInt(damage * 0.3f);
            currentHealth -= reducedDamage;

            // Nếu giáp bị âm (tức damage vượt quá lượng giáp hiện có)
            if (currentArmor < 0)
            {
                // Phần vượt quá giáp được tính full cho máu
                int extraDamage = -currentArmor; // tương đương với damage - giáp ban đầu
                currentHealth -= extraDamage;
                currentArmor = 0; // đảm bảo giáp không âm
            }
        }
        else
        {
            // Khi không có giáp, máu nhận damage đầy đủ
            currentHealth -= damage;
        }

        // Giữ cho máu không âm
        if (currentHealth <= 0)
            currentHealth = 0;

        Debug.Log("Current Armor: " + currentArmor + " - Current Health: " + currentHealth);
    }
}
