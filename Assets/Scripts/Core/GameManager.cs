using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    [Header("UI")]
    [SerializeField] private GameObject winCanvas;
    [SerializeField] private GameObject loseCanvas;

    [Header("HUD")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text killsText;
    [SerializeField] private TMP_Text timerText;

    [Header("Boss UI")]
    [SerializeField] private TMP_Text bossMessageText;

    private int bossesKilled = 0;
    private int bossesToKill = 1;

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

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Stage1")
        {
            SetBossRequirement(1);
            StartGame();
        }

        if (scene.name == "Stage2")
        {
            SetBossRequirement(2);
            StartGame();
        }
    }

    void Update()
    {
        if (CurrentState != GameState.Playing)
            return;

        TimeSurvived += Time.deltaTime;
        UpdateTimerUI();
    }

    // ---------------- GAME FLOW ----------------

    public void StartGame()
    {
        Score = 0;
        EnemiesKilled = 0;
        TimeSurvived = 0f;

        bossesKilled = 0;

        CurrentState = GameState.Playing;
        Time.timeScale = 1f;

        HideAllUI();
        UpdateHUD();
        UpdateTimerUI();

        OnStateChanged?.Invoke(CurrentState);
    }

    public void GameOver()
    {
        if (CurrentState != GameState.Playing)
            return;

        CurrentState = GameState.GameOver;
        Time.timeScale = 0f;

        ShowLoseUI();
        SaveSession();

        OnStateChanged?.Invoke(CurrentState);
    }

    public void GameWon()
    {
        if (CurrentState != GameState.Playing)
            return;

        CurrentState = GameState.Win;
        Time.timeScale = 0f;

        ShowWinUI();
        SaveSession();

        OnStateChanged?.Invoke(CurrentState);
    }

    // ---------------- BOSS SYSTEM ----------------

    public void SetBossRequirement(int amount)
    {
        bossesToKill = amount;
        bossesKilled = 0;
    }

    public void RegisterBossKill()
    {
        bossesKilled++;

        if (bossesKilled >= bossesToKill)
        {
            GameWon();
        }
    }

    public void ShowBossMessage(string message, float time = 3f)
    {
        if (bossMessageText == null) return;

        bossMessageText.text = message;
        bossMessageText.gameObject.SetActive(true);

        CancelInvoke(nameof(HideBossMessage));
        Invoke(nameof(HideBossMessage), time);
    }

    void HideBossMessage()
    {
        if (bossMessageText != null)
            bossMessageText.gameObject.SetActive(false);
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

    // ---------------- LEADERBOARD ----------------

    void SaveSession()
    {
        if (LeaderboardManager.Instance != null)
        {
            LeaderboardManager.Instance.AddSession(
                TimeSurvived,
                EnemiesKilled,
                Score
            );
        }
    }

    // ---------------- UI ----------------

    void UpdateHUD()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + Score;

        if (killsText != null)
            killsText.text = "Kills: " + EnemiesKilled;
    }

    void UpdateTimerUI()
    {
        if (timerText == null) return;

        int minutes = Mathf.FloorToInt(TimeSurvived / 60);
        int seconds = Mathf.FloorToInt(TimeSurvived % 60);

        timerText.text = $"{minutes:00}:{seconds:00}";
    }

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
}