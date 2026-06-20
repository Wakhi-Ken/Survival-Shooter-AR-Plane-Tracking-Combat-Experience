using UnityEngine;

public class MedKit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Touched: " + other.name);

        Health playerHealth =
            other.GetComponentInParent<Health>();

        if (playerHealth != null)
        {
            Debug.Log("Medkit collected!");

            if (MedKitInventory.Instance != null)
            {
                MedKitInventory.Instance.AddMedKit(1);
            }

            Destroy(gameObject);
        }
    }
}