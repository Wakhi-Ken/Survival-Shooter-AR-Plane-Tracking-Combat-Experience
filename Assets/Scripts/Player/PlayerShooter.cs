using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    public Transform firePoint;

    public void Shoot()
    {
        GameObject bullet = BulletPool.Instance.GetBullet();
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = firePoint.rotation;
        bullet.SetActive(true);
    }
}