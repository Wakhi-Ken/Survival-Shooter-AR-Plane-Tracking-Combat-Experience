using UnityEngine;

public class BossEnemy : EnemyBase
{
    public float speed = 1f;
    public float range = 2f;
    public float shootRange = 7f;

    float timer;

    public GameObject bullet;

    void Update()
    {
        float dist = Vector3.Distance(transform.position, player.position);

        if (dist > shootRange)
        {
            MoveToPlayer(speed);
        }
        else
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                if (dist < range)
                    MeleeAttack();
                else
                    Shoot();

                timer = 1.5f;
            }
        }
    }

    void MeleeAttack()
    {
        player.GetComponent<PlayerHealth>().TakeDamage(2);
    }

    void Shoot()
    {
        GameObject b = Instantiate(bullet);
        b.transform.position = transform.position;
        b.transform.LookAt(player);

        player.GetComponent<PlayerHealth>().TakeDamage(3);
    }
}