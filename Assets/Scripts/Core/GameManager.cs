// Assets/Scripts/Managers/GameManager.cs
using UnityEngine;
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

    public event Action<GameState> OnStateChanged;
    public event Action<int> OnScoreChanged;
    public event Action<int> OnEnemiesKilledChanged;
    public event Action<float> OnTimeUpdated;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // ensure UI starts hidden
        if (winCanvas) winCanvas.SetActive(false);
        if (loseCanvas) loseCanvas.SetActive(false);
    }

    void Update()
    {
        if (CurrentState == GameState.Playing)
        {
            timeRemaining -= Time.deltaTime;
            TimeSurvived += Time.deltaTime;

            OnTimeUpdated?.Invoke(timeRemaining);

            if (timeRemaining <= 0)
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

        OnStateChanged?.Invoke(CurrentState);

        LeaderboardManager.Instance?.AddScore(Score, EnemiesKilled, TimeSurvived);
    }

    public void GameWon()
    {
        if (CurrentState != GameState.Playing)
            return;

        CurrentState = GameState.Win;

        Time.timeScale = 0f;

        ShowWinUI();

        OnStateChanged?.Invoke(CurrentState);

        LeaderboardManager.Instance?.AddScore(Score, EnemiesKilled, TimeSurvived);
    }

    void ResetGame()
    {
        Score = 0;
        EnemiesKilled = 0;
        TimeSurvived = 0f;
        timeRemaining = gameDuration;
    }

    // ---------------- UI HANDLING ----------------

    void HideAllUI()
    {
        if (winCanvas) winCanvas.SetActive(false);
        if (loseCanvas) loseCanvas.SetActive(false);
    }

    void ShowWinUI()
    {
        if (winCanvas) winCanvas.SetActive(true);
        if (loseCanvas) loseCanvas.SetActive(false);
    }

    void ShowLoseUI()
    {
        if (loseCanvas) loseCanvas.SetActive(true);
        if (winCanvas) winCanvas.SetActive(false);
    }

    // ---------------- STATS ----------------

    public void AddScore(int amount)
    {
        Score += amount;
        OnScoreChanged?.Invoke(Score);
    }

    public void AddKill()
    {
        EnemiesKilled++;
        OnEnemiesKilledChanged?.Invoke(EnemiesKilled);
    }
}