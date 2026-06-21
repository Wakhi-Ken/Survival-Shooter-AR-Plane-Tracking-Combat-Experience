using TMPro;
using UnityEngine;

public class HUDBinder : MonoBehaviour
{
    [Header("HUD")]
    public TMP_Text scoreText;
    public TMP_Text killsText;
    public TMP_Text timerText;

    [Header("Boss UI")]
    public TMP_Text bossMessageText;

    private void Start()
    {
        if (GameManager.Instance == null)
            return;

        GameManager.Instance.RebindUI(
            scoreText,
            killsText,
            timerText
        );

        GameManager.Instance.RebindBossUI(
            bossMessageText
        );
    }
}