using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 150;
    public int armor = 0;
    public int medkits = 2;

    public Slider healthSlider;

    int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateUI();
    }

    public void TakeDamage(int dmg)
    {
        int finalDamage = dmg;

        if (armor > 0)
        {
            armor--;
            finalDamage = Mathf.Max(0, dmg - 1);
        }

        currentHealth -= finalDamage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateUI();

        if (currentHealth <= 0)
        {
            GameManager.Instance.GameOver(false);
        }
    }

    public void UseMedkit()
    {
        if (medkits <= 0) return;

        medkits--;
        currentHealth += 40;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateUI();
    }

    public void AddArmor(int value)
    {
        armor += value;
        UpdateUI();
    }

    void UpdateUI()
    {
        healthSlider.value = currentHealth;
    }
}