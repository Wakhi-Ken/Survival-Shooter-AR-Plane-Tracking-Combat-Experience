using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { Menu, Playing, GameOver }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState State;

    [Header("UI")]
    public GameObject menuUI;
    public GameObject gameUI;
    public GameObject gameOverUI;

    [Header("Timer")]
    public float gameTime = 120f;
    float timer;
    bool running;

    [Header("Systems")]
    public EnemySpawner enemySpawner;   // 🔥 NEW

    bool gameEnded;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        SetState(GameState.Menu);
    }

    void Update()
    {
        if (!running || gameEnded) return;

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            GameOver(false); // time out = lose
        }
    }

    // ---------------- STATE ----------------

    public void SetState(GameState state)
    {
        State = state;

        menuUI.SetActive(state == GameState.Menu);
        gameUI.SetActive(state == GameState.Playing);
        gameOverUI.SetActive(state == GameState.GameOver);

        if (state == GameState.Playing)
            StartGame();
    }

    // ---------------- GAME FLOW ----------------

    void StartGame()
    {
        timer = gameTime;
        running = true;
        gameEnded = false;

        ScoreManager.Instance.ResetScore();

        if (enemySpawner != null)
            enemySpawner.StartSpawning();
    }

    public void GameOver(bool win)
    {
        if (gameEnded) return;

        gameEnded = true;
        running = false;

        Debug.Log(win ? "YOU WIN" : "YOU LOSE");

        if (enemySpawner != null)
            enemySpawner.StopSpawning();

        SetState(GameState.GameOver);

        // OPTIONAL (leaderboard hook later)
        //Leaderboard.Instance.SaveScore(ScoreManager.Instance.score);
    }

    // ---------------- UI BUTTONS ----------------

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // ---------------- TIMER ACCESS ----------------

    public float GetTimeRemaining()
    {
        return timer;
    }
}