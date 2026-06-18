using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    [Header("Base Stats")]
    public int maxHealth = 100;
    protected int currentHealth;

    [Header("Score")]
    public int scoreValue = 10;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;

        Debug.Log(gameObject.name + " HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Debug.Log(gameObject.name + " died");
        Destroy(gameObject);
    }
}