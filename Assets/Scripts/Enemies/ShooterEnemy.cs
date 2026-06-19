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

    private Transform player;
    private float lastShotTime;

    protected override void Start()
    {
        maxHealth = 120;
        base.Start();

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(
            transform.position,
            player.position
        );

        if (distance > stopDistance)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                player.position,
                moveSpeed * Time.deltaTime
            );
        }
        else
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (Time.time < lastShotTime + fireRate)
            return;

        lastShotTime = Time.time;

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
}