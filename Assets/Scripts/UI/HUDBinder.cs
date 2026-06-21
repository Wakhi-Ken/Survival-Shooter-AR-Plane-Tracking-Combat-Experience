using TMPro;
using UnityEngine;

public class HUDBinder : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text scoreText;
    public TMP_Text killsText;
    public TMP_Text timerText;

    void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RebindUI(scoreText, killsText, timerText);
        }
    }
}