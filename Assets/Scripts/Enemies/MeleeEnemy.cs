using UnityEngine;
using System.Collections;

public class MeleeEnemy : BaseEnemy
{
    public float moveSpeed = 2f;
    public float stopDistance = 1.5f;

    public float attackRange = 4f;
    public int damage = 10;
    public float attackCooldown = 1f;

    public Animator meleeAnimator;

    private Transform player;
    private float lastAttackTime;

    protected override void Start()
    {
        base.Start();

        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (meleeAnimator == null)
            meleeAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isDead || player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            Attack();

            if (distance > stopDistance)
                Move();
        }
        else
        {
            Move();
        }
    }

    void Move()
    {
        if (isDead) return;

        if (meleeAnimator != null)
            meleeAnimator.SetBool("IsWalking", true);

        transform.position = Vector3.MoveTowards(
            transform.position,
            player.position,
            moveSpeed * Time.deltaTime
        );
    }

    void Attack()
    {
        if (Time.time < lastAttackTime + attackCooldown) return;

        lastAttackTime = Time.time;

        if (meleeAnimator != null)
            meleeAnimator.SetTrigger("Attack");

        Health hp = player.GetComponentInChildren<Health>();
        if (hp != null)
            hp.TakeDamage(damage);
    }

    // OPTIONAL: ensures animation state is clean when dying
    protected override void Die()
    {
        base.Die();

        if (meleeAnimator != null)
        {
            meleeAnimator.SetBool("IsWalking", false);
            meleeAnimator.SetTrigger("Die");
        }
    }
}