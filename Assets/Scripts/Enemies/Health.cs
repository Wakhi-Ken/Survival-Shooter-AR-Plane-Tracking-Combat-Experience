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

    [Header("Damage Indicator")]
    public Image damageIndicator;
    public float maxAlpha = 1f;
    public float fadeSpeed = 2f;

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
        

        ShowDamageIndicator();

        UpdateUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void ShowDamageIndicator()
    {
        if (damageIndicator == null) return;

        Color c = damageIndicator.color;
        c.a = maxAlpha;

        damageIndicator.color = c;
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

    void Update()
    {
        if (damageIndicator == null) return;

        Color c = damageIndicator.color;

        if (c.a > 0)
        {
            c.a -= fadeSpeed * Time.deltaTime;

            if (c.a < 0)
                c.a = 0;

            damageIndicator.color = c;
        }
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