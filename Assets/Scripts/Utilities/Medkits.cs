using UnityEngine;

public class MedKit : MonoBehaviour
{
    public int healAmount = 25;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Touched: " + other.name);

        Health playerHealth =
            other.GetComponentInParent<Health>();

        if (playerHealth != null)
        {
            Debug.Log("Player collected medkit!");

            playerHealth.Heal(healAmount);

            Destroy(gameObject);
        }
    }
}