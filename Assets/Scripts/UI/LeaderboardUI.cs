using TMPro;
using UnityEngine;

public class LeaderboardUI : MonoBehaviour
{
    public TMP_Text displayText;
    public GameObject panel;

    public void OpenLeaderboard()
    {
        panel.SetActive(true);
        Refresh();
    }

    public void CloseLeaderboard()
    {
        panel.SetActive(false);
    }

    public void Refresh()
    {
        if (LeaderboardManager.Instance == null)
        {
            displayText.text = "No data yet.";
            return;
        }

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