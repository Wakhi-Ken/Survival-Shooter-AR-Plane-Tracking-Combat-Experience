using UnityEngine;
using TMPro;
using System;

public enum GameState
{
    Start,
    Playing,
    GameOver,
    Win
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameState CurrentState { get; private set; } = GameState.Start;

    public int Score { get; private set; }
    public int EnemiesKilled { get; private set; }
    public float TimeSurvived { get; private set; }

    [Header("Game Settings")]
    [SerializeField] private float gameDuration = 60f;

    private float timeRemaining;

    [Header("UI Canvases")]
    [SerializeField] private GameObject winCanvas;
    [SerializeField] private GameObject loseCanvas;

    [Header("HUD")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text killsText;
    [SerializeField] private TMP_Text timerText;

    public event Action<GameState> OnStateChanged;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (winCanvas != null)
            winCanvas.SetActive(false);

        if (loseCanvas != null)
            loseCanvas.SetActive(false);

        UpdateHUD();
    }

    void Update()
    {
        if (CurrentState != GameState.Playing)
            return;

        timeRemaining -= Time.deltaTime;
        TimeSurvived += Time.deltaTime;

        if (timeRemaining < 0)
            timeRemaining = 0;

        UpdateTimerUI();

        if (timeRemaining <= 0)
        {
            GameOver();
        }
    }

    // ---------------- GAME FLOW ----------------

    public void StartGame()
    {
        ResetGame();

        CurrentState = GameState.Playing;

        Time.timeScale = 1f;

        HideAllUI();

        OnStateChanged?.Invoke(CurrentState);
    }

    public void GameOver()
    {
        if (CurrentState != GameState.Playing)
            return;

        CurrentState = GameState.GameOver;

        Time.timeScale = 0f;

        ShowLoseUI();

        LeaderboardManager.Instance?.AddScore(
            Score,
            EnemiesKilled,
            TimeSurvived
        );

        OnStateChanged?.Invoke(CurrentState);
    }

    public void GameWon()
    {
        if (CurrentState != GameState.Playing)
            return;

        CurrentState = GameState.Win;

        Time.timeScale = 0f;

        ShowWinUI();

        LeaderboardManager.Instance?.AddScore(
            Score,
            EnemiesKilled,
            TimeSurvived
        );

        OnStateChanged?.Invoke(CurrentState);
    }

    void ResetGame()
    {
        Score = 0;
        EnemiesKilled = 0;
        TimeSurvived = 0f;

        timeRemaining = gameDuration;

        UpdateHUD();
        UpdateTimerUI();
    }

    // ---------------- SCORE ----------------

    public void AddScore(int amount)
    {
        Score += amount;

        UpdateHUD();
    }

    public void AddKill()
    {
        EnemiesKilled++;

        UpdateHUD();
    }

    // ---------------- UI ----------------

    void UpdateHUD()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + Score;
        }

        if (killsText != null)
        {
            killsText.text = "Kills: " + EnemiesKilled;
        }
    }

    void UpdateTimerUI()
    {
        if (timerText == null)
            return;

        int minutes =
            Mathf.FloorToInt(timeRemaining / 60);

        int seconds =
            Mathf.FloorToInt(timeRemaining % 60);

        timerText.text =
            string.Format("{0:00}:{1:00}",
            minutes,
            seconds);
    }

    void HideAllUI()
    {
        if (winCanvas != null)
            winCanvas.SetActive(false);

        if (loseCanvas != null)
            loseCanvas.SetActive(false);
    }

    void ShowWinUI()
    {
        if (winCanvas != null)
            winCanvas.SetActive(true);

        if (loseCanvas != null)
            loseCanvas.SetActive(false);
    }

    void ShowLoseUI()
    {
        if (loseCanvas != null)
            loseCanvas.SetActive(true);

        if (winCanvas != null)
            winCanvas.SetActive(false);
    }

    // ---------------- GETTERS ----------------

    public float GetTimeRemaining()
    {
        return timeRemaining;
    }

    public float GetTimeSurvived()
    {
        return TimeSurvived;
    }
}