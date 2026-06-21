using TMPro;
using UnityEngine;

public class HUDBinder : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text scoreText;
    public TMP_Text killsText;
    public TMP_Text timerText;

    [Header("Boss UI")]
    public TMP_Text bossMessageText;

    void Start()
    {
        if (GameManager.Instance != null)
        {
            // HUD bind
            GameManager.Instance.RebindUI(scoreText, killsText, timerText);

            // Boss UI bind
            GameManager.Instance.RebindBossUI(bossMessageText);
        }
    }
}