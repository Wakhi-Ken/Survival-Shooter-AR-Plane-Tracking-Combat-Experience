using UnityEngine;

public class BossEnemy : BaseEnemy
{
    [Header("Movement")]
    public float moveSpeed = 3f;

    [Header("Ranges")]
    public float meleeRange = 2f;

    [Header("Melee")]
    public int meleeDamage = 25;
    public float meleeCooldown = 2f;

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform shootPoint;
    public float bulletSpeed = 20f;
    public float shootCooldown = 0.8f;

    [Header("Animator")]
    public Animator bossAnimator;

    private Transform player;
    private float lastShootTime;
    private float lastMeleeTime;

    protected override void Start()
    {
        maxHealth = 250;
        scoreValue = 100;

        base.Start();

        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (bossAnimator == null)
            bossAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isDead || player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        Move();
        Shoot();

        if (distance <= meleeRange)
        {
            Melee();
        }
    }

    // ---------------- MOVE ----------------
    void Move()
    {
        if (isDead) return;

        if (bossAnimator != null)
            bossAnimator.SetBool("IsWalking", true);

        transform.position = Vector3.MoveTowards(
            transform.position,
            player.position,
            moveSpeed * Time.deltaTime
        );
    }

    // ---------------- SHOOT ----------------
    void Shoot()
    {
        if (Time.time < lastShootTime + shootCooldown)
            return;

        lastShootTime = Time.time;

        if (bossAnimator != null)
            bossAnimator.SetTrigger("Shoot");

        if (bulletPrefab == null || shootPoint == null)
            return;

        GameObject bullet = Instantiate(
            bulletPrefab,
            shootPoint.position,
            Quaternion.identity
        );

        Vector3 direction = (player.position - shootPoint.position).normalized;

        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null)
            rb.linearVelocity = direction * bulletSpeed;
    }

    // ---------------- MELEE ----------------
    void Melee()
    {
        if (Time.time < lastMeleeTime + meleeCooldown)
            return;

        lastMeleeTime = Time.time;

        if (bossAnimator != null)
            bossAnimator.SetTrigger("Melee");

        Health playerHealth = player.GetComponentInChildren<Health>();

        if (playerHealth != null)
            playerHealth.TakeDamage(meleeDamage);
    }

    // ---------------- DEATH ----------------
    protected override void Die()
    {
        if (isDead) return;

        isDead = true;

        if (bossAnimator != null)
        {
            bossAnimator.SetBool("IsWalking", false);
            bossAnimator.ResetTrigger("Shoot");
            bossAnimator.ResetTrigger("Melee");
            bossAnimator.SetTrigger("Die");
        }

        // ✅ SAFE GAME SYSTEM UPDATE (NO ANIMATION IMPACT)
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(scoreValue);
            GameManager.Instance.AddKill();

            GameManager.Instance.RegisterBossKill();
        }

        // delay destroy so animation plays
        Destroy(gameObject, 2.5f);
    }
}