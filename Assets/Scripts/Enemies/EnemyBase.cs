using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    public int health;
    public int damage;

    protected Transform player;

    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public virtual void TakeDamage(int dmg)
    {
        health -= dmg;

        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        ScoreManager.Instance.AddScore(10);
        Destroy(gameObject);
    }

    protected void MoveToPlayer(float speed)
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            player.position,
            speed * Time.deltaTime
        );
    }
}