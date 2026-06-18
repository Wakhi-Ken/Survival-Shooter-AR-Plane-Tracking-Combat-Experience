using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Damage Settings")]
    public int damage = 25;
    public float lifeTime = 3f;


    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
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
        // Try to damage anything with Health
        Health health = collision.collider.GetComponent<Health>();

        if (health != null)
        {
            health.TakeDamage(damage);
        }

        gameObject.SetActive(false);
    }
}