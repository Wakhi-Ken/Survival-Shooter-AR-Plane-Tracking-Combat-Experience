using UnityEngine;

public class MeleeEnemy : BaseEnemy
{
    [Header("Movement")]
    public float moveSpeed = 2f;
    public float stopDistance = 1.5f;

    [Header("Attack")]
    public int damage = 10;
    public float attackCooldown = 1f;

    private Transform player;
    private float lastAttackTime;

    protected override void Start()
    {
        maxHealth = 150;
        scoreValue = 10;

        base.Start();

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (player == null) return;

        float distance =
            Vector3.Distance(transform.position, player.position);

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
            Attack();
        }
    }

    void Attack()
    {
        if (Time.time < lastAttackTime + attackCooldown)
            return;

        lastAttackTime = Time.time;

        Health playerHealth =
            player.GetComponent<Health>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }
    }
}