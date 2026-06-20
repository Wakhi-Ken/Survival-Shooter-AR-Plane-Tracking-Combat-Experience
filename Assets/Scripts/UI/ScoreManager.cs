using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("UI")]
    public TMP_Text scoreText;
    public TMP_Text killsText;

    private int score;
    private int enemiesKilled;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateUI();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateUI();
    }

    public void AddKill()
    {
        enemiesKilled++;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;

        if (killsText != null)
            killsText.text = "Kills: " + enemiesKilled;
    }

    public int GetScore()
    {
        return score;
    }

    public int GetKills()
    {
        return enemiesKilled;
    }
}