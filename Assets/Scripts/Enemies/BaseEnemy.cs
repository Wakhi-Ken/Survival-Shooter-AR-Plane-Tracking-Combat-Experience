using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    public int maxHealth = 100;
    protected int currentHealth;

    public int scoreValue = 10;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(scoreValue);
            GameManager.Instance.AddKill();
        }

        Destroy(gameObject);
    }
}