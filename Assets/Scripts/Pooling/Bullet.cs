using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 15f;
    public float life = 3f;

    void OnEnable()
    {
        Invoke("Disable", life);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void Disable()
    {
        gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        EnemyBase enemy = other.GetComponent<EnemyBase>();

        if (enemy != null)
        {
            enemy.TakeDamage(25);
            gameObject.SetActive(false);
        }
    }
}