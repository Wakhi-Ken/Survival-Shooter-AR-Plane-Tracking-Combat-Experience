using UnityEngine;
using TMPro;

public class MedkitsInventory : MonoBehaviour
{
    [Header("Medkit Count")]
    public int medkitCount = 0;

    [Header("UI")]
    public TMP_Text medkitText;

    [Header("Heal Settings")]
    public Health playerHealth;
    public int healAmount = 25;

    void Start()
    {
        UpdateUI();

        if (playerHealth == null)
            playerHealth = GetComponent<Health>();
    }

    // ---------------- PICKUP ----------------
    public void AddMedkit(int amount)
    {
        medkitCount += amount;
        UpdateUI();
    }

    // ---------------- USE MEDKIT ----------------
    public void UseMedkit()
    {
        if (medkitCount <= 0)
        {
            Debug.Log("No medkits!");
            return;
        }

        medkitCount--;

        if (playerHealth != null)
            playerHealth.Heal(healAmount);

        UpdateUI();
    }

    // ---------------- UI ----------------
    void UpdateUI()
    {
        if (medkitText != null)
            medkitText.text = "Medkits: " + medkitCount;
    }
}