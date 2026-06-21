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

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        if (CurrentState != GameState.Playing)
            return;

        TimeSurvived += Time.deltaTime;
        UpdateTimerUI();
    }

    // ---------------- SCENE LOAD ----------------

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Stage1")
        {
            ResetBossSystem(1);
            StartGame();
        }
        else if (scene.name == "Stage2")
        {
            ResetBossSystem(2);
            StartGame();
        }
    }

    // ---------------- HUD BINDING ----------------

    public void RebindUI(TMP_Text score, TMP_Text kills, TMP_Text timer)
    {
        scoreText = score;
        killsText = kills;
        timerText = timer;

        UpdateHUD();
        UpdateTimerUI();
    }

    public void RebindBossUI(TMP_Text bossText)
    {
        bossMessageText = bossText;

        if (bossMessageText != null)
            bossMessageText.gameObject.SetActive(false);
    }

    // ---------------- GAME FLOW ----------------

    public void StartGame()
    {
        Score = 0;
        EnemiesKilled = 0;
        TimeSurvived = 0f;

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

    // ---------------- BOSS SYSTEM ----------------

    public void ResetBossSystem(int amount)
    {
        bossesToKill = amount;
        bossesKilled = 0;
    }

    public void RegisterBossKill()
    {
        if (CurrentState != GameState.Playing)
            return;

        bossesKilled++;

        if (bossesKilled >= bossesToKill)
        {
            GameWon();
        }
    }

    public void ShowBossMessage(string message, float duration = 3f)
    {
        if (bossMessageText == null)
            return;

        bossMessageText.text = message;
        bossMessageText.gameObject.SetActive(true);

        CancelInvoke(nameof(HideBossMessage));
        Invoke(nameof(HideBossMessage), duration);
    }

    private void HideBossMessage()
    {
        if (bossMessageText != null)
            bossMessageText.gameObject.SetActive(false);
    }

    // ---------------- LEADERBOARD ----------------

    private void SaveSession()
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

    private void UpdateHUD()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + Score;

        if (killsText != null)
            killsText.text = "Kills: " + EnemiesKilled;
    }

    private void UpdateTimerUI()
    {
        if (timerText == null)
            return;

        int minutes = Mathf.FloorToInt(TimeSurvived / 60f);
        int seconds = Mathf.FloorToInt(TimeSurvived % 60f);

        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    private void HideAllUI()
    {
        if (winCanvas != null)
            winCanvas.SetActive(false);

        if (loseCanvas != null)
            loseCanvas.SetActive(false);
    }

    private void ShowWinUI()
    {
        if (winCanvas != null)
            winCanvas.SetActive(true);

        if (loseCanvas != null)
            loseCanvas.SetActive(false);
    }

    private void ShowLoseUI()
    {
        if (loseCanvas != null)
            loseCanvas.SetActive(true);

        if (winCanvas != null)
            winCanvas.SetActive(false);
    }
}