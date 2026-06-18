// Assets/Scripts/Managers/LeaderboardManager.cs
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
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        savePath = Path.Combine(Application.persistentDataPath, "leaderboard.json");
        LoadScores();
    }

    public void AddScore(int score, int enemiesKilled, float timeSurvived)
    {
        ScoreEntry entry = new ScoreEntry
        {
            score = score,
            enemiesKilled = enemiesKilled,
            timeSurvived = timeSurvived,
            date = DateTime.Now.ToString("yyyy-MM-dd HH:mm")
        };

        data.scores.Add(entry);

        // Keep only latest 10
        if (data.scores.Count > 10)
            data.scores.RemoveAt(0);

        SaveScores();
    }

    public List<ScoreEntry> GetTopScores(int count)
    {
        data.scores.Sort((a, b) => b.score.CompareTo(a.score));
        return data.scores.GetRange(0, Mathf.Min(count, data.scores.Count));
    }

    void SaveScores()
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(savePath, json);
    }

    void LoadScores()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            data = JsonUtility.FromJson<LeaderboardData>(json);
        }
        else
        {
            data = new LeaderboardData();
        }
    }
}