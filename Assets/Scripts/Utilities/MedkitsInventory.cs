using UnityEngine;
using TMPro;

public class MedKitInventory : MonoBehaviour
{
    [Header("Medkit Count")]
    public int medkitCount = 1; // Start with 1 medkit

    [Header("UI")]
    public TMP_Text medkitText;

    [Header("Heal Settings")]
    public Health playerHealth;
    public int healAmount = 25;

    void Start()
    {
        if (playerHealth == null)
            playerHealth = GetComponent<Health>();

        UpdateUI();
    }

    public void AddMedkit(int amount)
    {
        medkitCount += amount;
        UpdateUI();
    }

    public void UseMedkit()
    {
        if (medkitCount <= 0)
            return;

        medkitCount--;

        if (playerHealth != null)
            playerHealth.Heal(healAmount);

        UpdateUI();
    }

    void UpdateUI()
    {
        if (medkitText != null)
            medkitText.text = "Medkits: " + medkitCount;
    }
}