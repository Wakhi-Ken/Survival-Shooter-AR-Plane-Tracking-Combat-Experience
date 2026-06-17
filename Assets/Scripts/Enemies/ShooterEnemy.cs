using UnityEngine;

public class ShooterEnemy : EnemyBase
{
    public float speed = 1.2f;
    public float shootRange = 6f;
    public float fireRate = 2f;

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
                Shoot();
                timer = fireRate;
            }
        }
    }

    void Shoot()
    {
        GameObject b = Instantiate(bullet);
        b.transform.position = transform.position;
        b.transform.LookAt(player);
    }
}