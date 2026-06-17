using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemies")]
    public GameObject meleeEnemy;
    public GameObject shooterEnemy;
    public GameObject bossEnemy;

    [Header("Spawn Points (optional AR planes or manual points)")]
    public Transform[] spawnPoints;

    [Header("Spawn Settings")]
    public float spawnRate = 3f;
    public float bossSpawnTime = 120f;

    float timer;
    float bossTimer;

    bool spawning;

    bool bossSpawned;

    void Update()
    {
        if (!spawning) return;

        timer -= Time.deltaTime;
        bossTimer += Time.deltaTime;

        // NORMAL ENEMY SPAWN
        if (timer <= 0f)
        {
            SpawnEnemy();
            timer = spawnRate;
        }

        // BOSS SPAWN AFTER 2 MINUTES
        if (!bossSpawned && bossTimer >= bossSpawnTime)
        {
            SpawnBoss();
            bossSpawned = true;
        }
    }

    // ---------------- CONTROL ----------------

    public void StartSpawning()
    {
        spawning = true;
        timer = spawnRate;
        bossTimer = 0f;
        bossSpawned = false;
    }

    public void StopSpawning()
    {
        spawning = false;
    }

    // ---------------- SPAWN LOGIC ----------------

    void SpawnEnemy()
    {
        if (spawnPoints.Length == 0) return;

        Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject enemyToSpawn =
            Random.value > 0.5f ? meleeEnemy : shooterEnemy;

        Instantiate(enemyToSpawn, point.position, point.rotation);
    }

    void SpawnBoss()
    {
        if (spawnPoints.Length == 0) return;

        Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];

        Instantiate(bossEnemy, point.position, point.rotation);

        Debug.Log("🔥 BOSS SPAWNED!");
    }
}