using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    public int maxHealth = 100;
    protected int currentHealth;

    public int scoreValue = 10;

    protected bool isDead = false;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("💀 DEAD: " + gameObject.name);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(scoreValue);
            GameManager.Instance.AddKill();
        }

        // Try play death animation if exists
        Animator anim = GetComponent<Animator>();
        if (anim != null)
        {
            //anim.SetTrigger("Die");
            anim.SetBool("IsWalking", false);
        }

        // stop movement/scripts safely
        foreach (var c in GetComponents<MonoBehaviour>())
        {
            if (c != this) c.enabled = false;
        }

        Destroy(gameObject, 2.5f);
    }
}