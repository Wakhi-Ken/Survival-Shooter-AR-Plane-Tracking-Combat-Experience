using UnityEngine;
using System.Collections;

public class MeleeEnemy : BaseEnemy
{
    [Header("Movement")]
    public float moveSpeed = 2f;
    public float stopDistance = 1.5f;

    [Header("Attack")]
    public float attackRange = 4f;
    public int damage = 10;
    public float attackCooldown = 1f;

    [Header("Animator")]
    public Animator meleeAnimator;

    [Header("Weapon Setup (OPTIONAL)")]
    public GameObject weaponPrefab;
    public Transform handSocket;

    [Header("Weapon Offset")]
    public Vector3 weaponPositionOffset;
    public Vector3 weaponRotationOffset;

    private GameObject currentWeapon;

    private Transform player;
    private float lastAttackTime;

    private bool isDead = false;

    protected override void Start()
    {
        maxHealth = 150;
        scoreValue = 10;

        base.Start();

        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        Debug.Log("🧠 Player found = " + (player != null));

        if (meleeAnimator == null)
            meleeAnimator = GetComponent<Animator>();

        AttachWeapon();
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

            return;
        }

        Move();
    }

    // ---------------- MOVE ----------------
    void Move()
    {
        if (meleeAnimator != null)
            

        transform.position = Vector3.MoveTowards(
            transform.position,
            player.position,
            moveSpeed * Time.deltaTime
        );
    }

    // ---------------- ATTACK ----------------
    void Attack()
    {
        if (meleeAnimator != null)
            meleeAnimator.SetBool("IsWalking", false);

        if (Time.time < lastAttackTime + attackCooldown)
            return;

        lastAttackTime = Time.time;

        Debug.Log("💥 ATTACK");

        if (meleeAnimator != null)
            meleeAnimator.SetTrigger("Attack");

        // ✅ FIXED PLAYER HEALTH SEARCH
        Health playerHealth = player.GetComponentInChildren<Health>();

        if (playerHealth != null)
        {
            Debug.Log("✅ Damage applied: " + damage);
            playerHealth.TakeDamage(damage);
        }
        else
        {
            Debug.Log("❌ Player Health NOT FOUND");
        }
    }

    // ---------------- WEAPON ----------------
    void AttachWeapon()
    {
        if (weaponPrefab == null || handSocket == null)
            return;

        currentWeapon = Instantiate(weaponPrefab, handSocket);

        currentWeapon.transform.localPosition = weaponPositionOffset;
        currentWeapon.transform.localRotation = Quaternion.Euler(weaponRotationOffset);
    }

    // ---------------- DEATH ----------------
    protected override void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("💀 DIE TRIGGERED");

        this.enabled = false;

        if (meleeAnimator != null)
        {
            meleeAnimator.SetBool("IsWalking", false);
            meleeAnimator.ResetTrigger("Attack");
            meleeAnimator.SetTrigger("Die");
        }

        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = false;

        Destroy(gameObject, 2f);
    }

    // ---------------- ANIMATION LENGTH ----------------
    float GetDeathAnimationLength()
    {
        if (meleeAnimator == null)
            return 2f;

        RuntimeAnimatorController ac = meleeAnimator.runtimeAnimatorController;

        foreach (AnimationClip clip in ac.animationClips)
        {
            if (clip.name.ToLower().Contains("die"))
                return clip.length;
        }

        return 2f;
    }
}