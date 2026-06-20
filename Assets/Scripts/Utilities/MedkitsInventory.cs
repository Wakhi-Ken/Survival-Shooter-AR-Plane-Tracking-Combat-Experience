using TMPro;
using UnityEngine;

public class MedKitInventory : MonoBehaviour
{
    public static MedKitInventory Instance;

    [Header("Inventory")]
    public int medKitCount = 1; // default 1

    [Header("UI")]
    public TMP_Text medKitText;

    [Header("Healing")]
    public Health playerHealth;
    public int healAmount = 25;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateUI();
    }

    public void AddMedKit(int amount)
    {
        medKitCount += amount;
        UpdateUI();

        Debug.Log("Medkits: " + medKitCount);
    }

    public void UseMedKit()
    {
        if (medKitCount <= 0)
        {
            Debug.Log("No medkits left!");
            return;
        }

        if (playerHealth != null)
        {
            medKitCount--;
            playerHealth.Heal(healAmount);

            UpdateUI();

            Debug.Log("Used medkit. Remaining: " + medKitCount);
        }
    }

    void UpdateUI()
    {
        if (medKitText != null)
            medKitText.text = medKitCount.ToString();
    }
}