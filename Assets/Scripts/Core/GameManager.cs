// Assets/Scripts/Managers/GameManager.cs
using UnityEngine;
using System;

public enum GameState
{
    Start,
    Playing,
    GameOver
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

    public void StartGame()
    {
        ResetGame();
        CurrentState = GameState.Playing;
        OnStateChanged?.Invoke(CurrentState);
    }

    public void GameOver()
    {
        CurrentState = GameState.GameOver;
        OnStateChanged?.Invoke(CurrentState);
        LeaderboardManager.Instance.AddScore(Score, EnemiesKilled, TimeSurvived);
    }

    void ResetGame()
    {
        Score = 0;
        EnemiesKilled = 0;
        TimeSurvived = 0;
        timeRemaining = gameDuration;
    }

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