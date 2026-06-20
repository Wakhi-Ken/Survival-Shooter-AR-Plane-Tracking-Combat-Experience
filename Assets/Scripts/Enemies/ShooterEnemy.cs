using UnityEngine;

public class ShooterEnemy : BaseEnemy
{
    [Header("Movement")]
    public float moveSpeed = 2f;
    public float stopDistance = 4f;

    [Header("Shooting")]
    public float fireRate = 1.2f;
    public int damage = 10;
    public GameObject bulletPrefab;
    public Transform shootPoint;
    public float bulletSpeed = 15f;

    [Header("Animator")]
    public Animator shooterAnimator;

    private Transform player;
    private float lastShotTime;

    private bool isDead = false;

    protected override void Start()
    {
        maxHealth = 120;
        scoreValue = 20;

        base.Start();

        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (shooterAnimator == null)
            shooterAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isDead || player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > stopDistance)
        {
            Move();
        }
        else
        {
            Shoot();
        }
    }

    // ---------------- MOVE ----------------
    void Move()
    {
        if (shooterAnimator != null)
            shooterAnimator.SetBool("IsWalking", true);

        transform.position = Vector3.MoveTowards(
            transform.position,
            player.position,
            moveSpeed * Time.deltaTime
        );
    }

    // ---------------- SHOOT ----------------
    void Shoot()
    {
        if (shooterAnimator != null)
            shooterAnimator.SetBool("IsWalking", false);

        if (Time.time < lastShotTime + fireRate)
            return;

        lastShotTime = Time.time;

        if (shooterAnimator != null)
            shooterAnimator.SetTrigger("Shoot");

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

    // ---------------- DEATH ----------------
    protected override void Die()
    {
        if (isDead) return;

        isDead = true;

        this.enabled = false;

        if (shooterAnimator != null)
        {
            shooterAnimator.SetBool("IsWalking", false);
            shooterAnimator.ResetTrigger("Shoot");
            shooterAnimator.SetTrigger("Die");
        }

        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = false;

        Destroy(gameObject, 2f);
    }
}