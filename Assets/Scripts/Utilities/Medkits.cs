using UnityEngine;

public class Medkit : MonoBehaviour
{
    public int healAmount = 25;
    public float rotateSpeed = 50f;

    void Update()
    {
        transform.Rotate(0f, rotateSpeed * Time.deltaTime, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        Health health = other.GetComponentInParent<Health>();

        if (health != null)
        {
            health.Heal(healAmount);
            Destroy(gameObject);
        }
        Debug.Log("Medkit touched by: " + other.name);
    }
}