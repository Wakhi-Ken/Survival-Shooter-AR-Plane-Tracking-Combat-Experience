using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [Header("Player Health")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("UI")]
    public Slider healthSlider;

    [Header("Death")]
    public PlayerAnimationController playerAnim;
    public SimpleGun gun;

    void Start()
    {
        currentHealth = maxHealth;

        if (playerAnim == null)
            playerAnim = GetComponent<PlayerAnimationController>();

        if (gun == null)
            gun = GetComponentInChildren<SimpleGun>();

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UpdateUI();
    }

    void UpdateUI()
    {
        if (healthSlider != null)
            healthSlider.value = currentHealth;
    }

    void Die()
    {
        // 🔥 play animation first
        if (playerAnim != null)
            playerAnim.PlayDie();

        // stop shooting
        if (gun != null)
            gun.enabled = false;

        // delay game over so animation can play
        Invoke(nameof(TriggerGameOver), 2f);
    }

    void TriggerGameOver()
    {
        gameObject.SetActive(false);

        if (GameManager.Instance != null)
            GameManager.Instance.GameOver();
    }
}