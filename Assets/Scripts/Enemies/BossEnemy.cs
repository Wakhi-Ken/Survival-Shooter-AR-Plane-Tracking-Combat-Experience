using UnityEngine;

public class BossEnemy : BaseEnemy
{
    [Header("Movement")]
    public float moveSpeed = 1.2f;
    public float strafeSpeed = 2f;

    [Header("Ranges")]
    public float meleeRange = 2f;

    [Header("Melee")]
    public int meleeDamage = 25;
    public float meleeCooldown = 2f;

    [Header("Shooting")]
    public float shootCooldown = 1.2f;
    public GameObject bulletPrefab;
    public Transform shootPoint;
    public float bulletSpeed = 20f;

    private Transform player;

    private float lastMeleeTime;
    private float lastShootTime;

    protected override void Start()
    {
        maxHealth = 250;
        base.Start();

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > meleeRange)
        {
            MoveBoss();
            Shoot();
        }
        else
        {
            MeleeAttack();
        }
    }

    // 👑 movement
    void MoveBoss()
    {
        Vector3 direction =
            (player.position - transform.position).normalized;

        Vector3 strafeDirection =
            Vector3.Cross(direction, Vector3.up);

        Vector3 move =
            direction * moveSpeed +
            strafeDirection * Mathf.Sin(Time.time * 2f) * strafeSpeed;

        transform.position += move * Time.deltaTime;
    }

    // 🔫 shooting
    void Shoot()
    {
        if (Time.time < lastShootTime + shootCooldown)
            return;

        lastShootTime = Time.time;

        if (bulletPrefab == null || shootPoint == null)
            return;

        shootPoint.LookAt(player);

        GameObject bullet = Instantiate(
            bulletPrefab,
            shootPoint.position,
            Quaternion.identity
        );

        Vector3 direction =
            (player.position - shootPoint.position).normalized;

        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = direction * bulletSpeed;
        }

        Destroy(bullet, 5f);
    }

    // 👊 melee
    void MeleeAttack()
    {
        if (Time.time < lastMeleeTime + meleeCooldown)
            return;

        lastMeleeTime = Time.time;

        Health playerHealth = player.GetComponent<Health>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(meleeDamage);
        }
    }

    // ---------------- IMPORTANT ADDITION ----------------

    protected override void Die()
    {
        // 🔥 WIN CONDITION TRIGGER
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameWon();
        }

        base.Die();
    }
}