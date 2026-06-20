using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;

[System.Serializable]
public class ScoreEntry
{
    public int score;
    public int enemiesKilled;
    public float timeSurvived;
    public string date;
}

[System.Serializable]
public class LeaderboardData
{
    public List<ScoreEntry> scores = new List<ScoreEntry>();
}

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager Instance { get; private set; }

    private LeaderboardData data;
    private string savePath;

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

        savePath = Path.Combine(
            Application.persistentDataPath,
            "leaderboard.json"
        );

        LoadScores();
    }

    public void AddScore(
        int score,
        int enemiesKilled,
        float timeSurvived)
    {
        ScoreEntry entry = new ScoreEntry
        {
            score = score,
            enemiesKilled = enemiesKilled,
            timeSurvived = timeSurvived,
            date = DateTime.Now.ToString("yyyy-MM-dd HH:mm")
        };

        data.scores.Add(entry);

        // Keep ONLY latest 5 sessions
        while (data.scores.Count > 5)
        {
            data.scores.RemoveAt(0);
        }

        SaveScores();
    }

    // Returns latest sessions
    public List<ScoreEntry> GetScores()
    {
        return data.scores;
    }

    // Returns highest scores first
    public List<ScoreEntry> GetTopScores()
    {
        List<ScoreEntry> sorted =
            new List<ScoreEntry>(data.scores);

        sorted.Sort((a, b) =>
            b.score.CompareTo(a.score));

        return sorted;
    }

    void SaveScores()
    {
        string json =
            JsonUtility.ToJson(data, true);

        File.WriteAllText(savePath, json);
    }

    void LoadScores()
    {
        if (File.Exists(savePath))
        {
            string json =
                File.ReadAllText(savePath);

            data =
                JsonUtility.FromJson<LeaderboardData>(json);

            if (data == null)
                data = new LeaderboardData();
        }
        else
        {
            data = new LeaderboardData();
        }
    }

    // Optional button for testing
    public void ClearLeaderboard()
    {
        data.scores.Clear();
        SaveScores();
    }
}