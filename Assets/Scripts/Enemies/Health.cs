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

    private bool isDead = false;

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
        if (isDead) return;

        currentHealth -= damage;
        UpdateUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        if (isDead) return;

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
        if (isDead) return;

        isDead = true;

        // play animation first
        if (playerAnim != null)
            playerAnim.PlayDie();

        // stop gameplay immediately
        if (gun != null)
            gun.enabled = false;

        // IMPORTANT: do NOT disable object yet
        Invoke(nameof(TriggerGameOver), 2f);
    }

    void TriggerGameOver()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.GameOver();

        // disable AFTER GameOver is triggered
        gameObject.SetActive(false);
    }
}