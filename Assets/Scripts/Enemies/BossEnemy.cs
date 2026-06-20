using UnityEngine;

public class BossEnemy : BaseEnemy
{
    [Header("Movement")]
    public float moveSpeed = 3f;
    public float strafeSpeed = 2.5f;

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

    private Transform player;

    private float lastShootTime;
    private float lastMeleeTime;

    protected override void Start()
    {
        maxHealth = 250;
        scoreValue = 100;

        base.Start();

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (player == null) return;

        FacePlayer();

        float distance =
            Vector3.Distance(transform.position, player.position);

        MoveForwardAndStrafe();
        Shoot();

        if (distance <= meleeRange)
        {
            MeleeAttack();
        }
    }

    // ---------------- LOOK AT PLAYER ----------------

    void FacePlayer()
    {
        Vector3 dir = player.position - transform.position;
        dir.y = 0;

        if (dir.sqrMagnitude > 0.01f)
            transform.rotation = Quaternion.LookRotation(dir);
    }

    // ---------------- SIMPLE MOVEMENT ----------------

    void MoveForwardAndStrafe()
    {
        Vector3 forward =
            (player.position - transform.position).normalized;

        Vector3 right =
            Vector3.Cross(forward, Vector3.up);

        float strafe = Mathf.Sin(Time.time * 2f);

        Vector3 move =
            forward * moveSpeed +
            right * strafe * strafeSpeed;

        transform.position += move * Time.deltaTime;
    }

    // ---------------- SHOOTING ----------------

    void Shoot()
    {
        if (Time.time < lastShootTime + shootCooldown)
            return;

        lastShootTime = Time.time;

        if (bulletPrefab == null || shootPoint == null)
            return;

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

    // ---------------- MELEE ----------------

    void MeleeAttack()
    {
        if (Time.time < lastMeleeTime + meleeCooldown)
            return;

        lastMeleeTime = Time.time;

        Health playerHealth =
            player.GetComponent<Health>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(meleeDamage);
        }
    }
}