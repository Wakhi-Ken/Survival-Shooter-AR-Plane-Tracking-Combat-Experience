using System.Collections.Generic;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    List<int> scores = new List<int>();

    public void SaveScore(int score)
    {
        scores.Add(score);

        if (scores.Count > 5)
            scores.RemoveAt(0);

        PlayerPrefs.SetString("scores", string.Join(",", scores));
    }
}