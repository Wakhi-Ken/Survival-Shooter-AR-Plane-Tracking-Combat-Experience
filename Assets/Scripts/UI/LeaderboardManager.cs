using System.Collections.Generic;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager Instance;

    private const string SAVE_KEY = "LEADERBOARD_DATA";

    public List<SessionData> sessions = new List<SessionData>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Load();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ---------------- ADD NEW RUN ----------------
    public void AddSession(float time, int kills, int score)
    {
        SessionData data = new SessionData
        {
            timeSurvived = time,
            kills = kills,
            score = score
        };

        sessions.Add(data);

        // keep only last 5
        if (sessions.Count > 5)
            sessions.RemoveAt(0);

        Save();
    }

    // ---------------- SAVE ----------------
    void Save()
    {
        string json = JsonUtility.ToJson(new Wrapper(sessions));
        PlayerPrefs.SetString(SAVE_KEY, json);
        PlayerPrefs.Save();
    }

    // ---------------- LOAD ----------------
    void Load()
    {
        if (!PlayerPrefs.HasKey(SAVE_KEY))
            return;

        string json = PlayerPrefs.GetString(SAVE_KEY);
        Wrapper wrapper = JsonUtility.FromJson<Wrapper>(json);

        sessions = wrapper.sessions ?? new List<SessionData>();
    }

    [System.Serializable]
    private class Wrapper
    {
        public List<SessionData> sessions;

        public Wrapper(List<SessionData> sessions)
        {
            this.sessions = sessions;
        }
    }
}