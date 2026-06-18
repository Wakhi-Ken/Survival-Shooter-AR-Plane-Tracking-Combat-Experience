using UnityEngine;

public class ShooterEnemy : BaseEnemy
{
    public float moveSpeed = 2f;
    public float shootRange = 6f;
    public float stopDistance = 4f;
    public float fireRate = 1.2f;

    public Transform shootPoint;
    public int damage = 10;

    private Transform player;
    private float lastShotTime;

    protected override void Start()
    {
        maxHealth = 120;   // 👈 SHOOTER HEALTH
        base.Start();

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

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

        Debug.Log("Shooter enemy shoots player");

        // later we will use OBJECT POOLING for enemy bullets here
        Health playerHealth = player.GetComponent<Health>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }
    }
}