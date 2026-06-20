using UnityEngine;
using TMPro;

public class GameHUD : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text scoreText;
    public TMP_Text killsText;
    public TMP_Text timerText;

    private GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.Instance;
    }

    void Update()
    {
        if (gameManager == null)
            return;

        // SCORE
        if (scoreText != null)
            scoreText.text = "Score: " + gameManager.Score;

        // KILLS
        if (killsText != null)
            killsText.text = "Kills: " + gameManager.EnemiesKilled;

        // TIMER (MM:SS format)
        float time = gameManager.GetTimeRemaining();

        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        if (timerText != null)
            timerText.text =
                string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}