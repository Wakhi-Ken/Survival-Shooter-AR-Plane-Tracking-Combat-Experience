using TMPro;
using UnityEngine;

public class LeaderboardUI : MonoBehaviour
{
    public TMP_Text displayText;

    void OnEnable()
    {
        ShowLeaderboard();
    }

    public void ShowLeaderboard()
    {
        if (LeaderboardManager.Instance == null) return;

        var list = LeaderboardManager.Instance.sessions;

        displayText.text = "RUN\tTIME\tKILLS\tSCORE\n";
        displayText.text += "--------------------------------------\n";

        for (int i = list.Count - 1; i >= 0; i--)
        {
            SessionData s = list[i];

            displayText.text +=
                $"#{i + 1}\t" +
                $"{s.timeSurvived:F1}s\t" +
                $"{s.kills}\t" +
                $"{s.score}\n";
        }
    }
}