using UnityEngine;

public class ShooterEnemy : BaseEnemy
{
    public float moveSpeed = 2f;
    public float stopDistance = 4f;

    public float fireRate = 1.2f;
    public GameObject bulletPrefab;
    public Transform shootPoint;
    public float bulletSpeed = 15f;

    public Animator shooterAnimator;

    private Transform player;
    private float lastShotTime;

    protected override void Start()
    {
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
            Move();
        else
            Shoot();
    }

    void Move()
    {
        if (isDead) return;

        if (shooterAnimator != null)
            shooterAnimator.SetBool("IsWalking", true);

        transform.position = Vector3.MoveTowards(
            transform.position,
            player.position,
            moveSpeed * Time.deltaTime
        );
    }

    void Shoot()
    {
        if (isDead) return;

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

        Vector3 dir = (player.position - shootPoint.position).normalized;

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
            rb.linearVelocity = dir * bulletSpeed;
    }

    // IMPORTANT: ensure clean animation trigger
    protected override void Die()
    {
        if (shooterAnimator != null)
        {
            shooterAnimator.SetBool("IsWalking", false);
            shooterAnimator.ResetTrigger("Shoot");
            shooterAnimator.SetTrigger("Die");
        }

        base.Die();
    }
}