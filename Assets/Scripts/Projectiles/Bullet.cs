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

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        Invoke(nameof(DisableBullet), lifeTime);
    }

    void DisableBullet()
    {
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasHit) return;
        hasHit = true;

        // 🎯 PRIORITY 1: Enemy
        BaseEnemy enemy = collision.collider.GetComponentInParent<BaseEnemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            gameObject.SetActive(false);
            return;
        }

        // 🎯 PRIORITY 2: Player
        Health playerHealth = collision.collider.GetComponentInParent<Health>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }

        gameObject.SetActive(false);
    }
}