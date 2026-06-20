using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    [Header("UI")]
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

        float timeRemaining = gameManager.GetTimeRemaining();

        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);

        timerText.text =
            string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}