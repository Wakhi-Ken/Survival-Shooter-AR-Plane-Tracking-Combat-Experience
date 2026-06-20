using UnityEngine;

public class MeleeEnemy : BaseEnemy
{
    public float moveSpeed = 2f;
    public float attackRange = 3f;
    public float attackCooldown = 1f;
    public int damage = 10;

    public Animator animator;

    private Transform player;
    private float lastAttack;
    private bool isDead;

    protected override void Start()
    {
        maxHealth = 150;
        scoreValue = 10;

        base.Start();

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        animator ??= GetComponent<Animator>();
    }

    void Update()
    {
        if (isDead || player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);

        if (dist > attackRange)
        {
            Move();
        }
        else
        {
            Attack();
        }
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

    void Attack()
    {
        if (Time.time < lastAttack + attackCooldown) return;

        lastAttack = Time.time;

        if (animator) animator.SetTrigger("Attack");

        Health hp = player.GetComponentInChildren<Health>();
        if (hp != null)
            hp.TakeDamage(damage);
    }

    protected override void Die()
    {
        if (isDead) return;
        isDead = true;

        base.Die();
    }
}