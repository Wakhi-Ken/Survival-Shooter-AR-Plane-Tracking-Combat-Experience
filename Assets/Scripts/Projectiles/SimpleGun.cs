using UnityEngine;
using TMPro;
using System.Collections;

public class SimpleGun : MonoBehaviour
{
    [Header("Bullet")]
    public GameObject bulletPrefab;
    public Transform shootPoint;

    [Header("Gun Settings")]
    public float bulletSpeed = 30f;
    public int magazineSize = 10;
    public float reloadTime = 2f;

    [Header("UI")]
    public TMP_Text ammoText;
    public TMP_Text reloadText;

    private int bulletsLeft;
    private bool isReloading;

    void Start()
    {
        bulletsLeft = magazineSize;

        if (reloadText != null)
            reloadText.gameObject.SetActive(false);

        UpdateUI();
    }

    public void Shoot()
    {
        if (isReloading)
            return;

        if (bulletsLeft <= 0)
        {
            Debug.Log("Out of ammo!");
            return;
        }

        bulletsLeft--;
        UpdateUI();

        GameObject bullet = BulletPool.Instance.GetBullet();

        bullet.transform.position = shootPoint.position;
        bullet.transform.rotation = shootPoint.rotation;

        bullet.SetActive(true);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = shootPoint.forward * bulletSpeed;
        }
    }

    public void Reload()
    {
        if (isReloading)
            return;

        if (bulletsLeft == magazineSize)
            return;

        StartCoroutine(ReloadRoutine());
    }

    IEnumerator ReloadRoutine()
    {
        isReloading = true;

        if (reloadText != null)
            reloadText.gameObject.SetActive(true);

        yield return new WaitForSeconds(reloadTime);

        bulletsLeft = magazineSize;

        UpdateUI();

        if (reloadText != null)
            reloadText.gameObject.SetActive(false);

        isReloading = false;
    }

    void UpdateUI()
    {
        if (ammoText != null)
            ammoText.text = bulletsLeft + " / " + magazineSize;
    }
}