using UnityEngine;

public class MeleeEnemy : EnemyBase
{
    public float speed = 1.5f;
    public float range = 1.5f;
    public float cooldown = 1f;

    float timer;

    void Update()
    {
        MoveToPlayer(speed);

        float dist = Vector3.Distance(transform.position, player.position);

        if (dist < range)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                player.GetComponent<PlayerHealth>().TakeDamage(1);
                timer = cooldown;
            }
        }
    }
}