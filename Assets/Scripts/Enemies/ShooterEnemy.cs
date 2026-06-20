using UnityEngine;

public class ShooterEnemy : BaseEnemy
{
    public float moveSpeed = 2f;
    public float stopDistance = 4f;

    public float fireRate = 1.2f;
    public GameObject bulletPrefab;
    public Transform shootPoint;
    public float bulletSpeed = 15f;

    public Animator animator;

    private Transform player;
    private float lastShotTime;
    private bool isDead;

    protected override void Start()
    {
        maxHealth = 120;
        scoreValue = 20;

        base.Start();

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        animator ??= GetComponent<Animator>();
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
        if (animator) animator.SetBool("IsWalking", true);

        transform.position = Vector3.MoveTowards(
            transform.position,
            player.position,
            moveSpeed * Time.deltaTime
        );
    }

    void Shoot()
    {
        if (Time.time < lastShotTime + fireRate) return;

        lastShotTime = Time.time;

        if (animator) animator.SetTrigger("Shoot");

        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);

        Vector3 dir = (player.position - shootPoint.position).normalized;

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
            rb.linearVelocity = dir * bulletSpeed;
    }

    protected override void Die()
    {
        if (isDead) return;
        isDead = true;

        base.Die();
    }
}