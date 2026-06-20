using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Damage Settings")]
    public int damage = 25;
    public float lifeTime = 3f;

    private Rigidbody rb;
    private bool hasHit;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        hasHit = false;
        CancelInvoke();

        // reset physics every reuse
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // auto return to pool after time
        Invoke(nameof(ReturnToPool), lifeTime);
    }

    void ReturnToPool()
    {
        if (BulletPool.Instance != null)
        {
            BulletPool.Instance.ReturnBullet(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasHit) return;
        hasHit = true;

        // 🎯 ENEMY HIT
        BaseEnemy enemy = collision.collider.GetComponentInParent<BaseEnemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            ReturnToPool();
            return;
        }

        // 🎯 PLAYER HIT
        Health playerHealth = collision.collider.GetComponentInParent<Health>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }

        ReturnToPool();
    }
}