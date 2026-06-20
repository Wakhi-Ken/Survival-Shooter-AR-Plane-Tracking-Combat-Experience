using System.Collections.Generic;
using UnityEngine;

public class ARPlaneSpawner : MonoBehaviour
{
    [Header("Game Ground")]
    public Transform gameGround;

    [Header("Enemy Prefabs")]
    public GameObject meleeEnemy;
    public GameObject shooterEnemy;
    public GameObject bossEnemy;

    [Header("Medkit")]
    public GameObject medkitPrefab;
    public float medkitSpawnInterval = 12f;
    public float medkitSpawnRadius = 3f;
    public int maxMedkits = 3;

    [Header("Spawn Settings")]
    public float spawnInterval = 5f;
    public int maxEnemies = 5;

    [Header("Spawn Heights")]
    public float meleeHeight = 1.2f;
    public float shooterHeight = 1.2f;
    public float bossHeight = 1.2f;
    public float medkitHeight = 1.0f;

    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private List<GameObject> spawnedMedkits = new List<GameObject>();

    private bool bossSpawned = false;

    void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 2f, spawnInterval);
        InvokeRepeating(nameof(SpawnMedkit), 3f, medkitSpawnInterval);

        Invoke(nameof(SpawnBoss), 30f);
    }

    // ---------------- ENEMIES ----------------
    void SpawnEnemy()
    {
        if (bossSpawned)
            return;

        CleanupDeadEnemies();

        if (spawnedEnemies.Count >= maxEnemies)
            return;

        if (gameGround == null)
        {
            Debug.LogWarning("Game Ground not assigned!");
            return;
        }

        Vector3 spawnPosition = GetRandomPointOnGround();

        GameObject enemyPrefab = GetRandomEnemy();

        if (enemyPrefab == meleeEnemy)
            spawnPosition.y += meleeHeight;
        else if (enemyPrefab == shooterEnemy)
            spawnPosition.y += shooterHeight;

        GameObject enemy =
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        spawnedEnemies.Add(enemy);
    }

    // ---------------- MEDKIT ----------------
    void SpawnMedkit()
    {
        if (medkitPrefab == null)
            return;

        CleanupDeadMedkits();

        if (spawnedMedkits.Count >= maxMedkits)
            return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
            return;

        Vector3 playerPos = player.transform.position;

        Vector2 randomCircle =
            Random.insideUnitCircle * medkitSpawnRadius;

        Vector3 spawnPosition = new Vector3(
            playerPos.x + randomCircle.x,
            gameGround.position.y + medkitHeight,
            playerPos.z + randomCircle.y
        );

        GameObject medkit =
            Instantiate(
                medkitPrefab,
                spawnPosition,
                Quaternion.identity
            );

        spawnedMedkits.Add(medkit);

        Debug.Log("🩹 Medkit spawned near player");
    }

    // ---------------- BOSS ----------------
    void SpawnBoss()
    {
        if (bossSpawned)
            return;

        bossSpawned = true;

        Vector3 spawnPosition = GetRandomPointOnGround();
        spawnPosition.y += bossHeight;

        GameObject boss =
            Instantiate(bossEnemy, spawnPosition, Quaternion.identity);

        spawnedEnemies.Add(boss);
    }

    // ---------------- GROUND POINT ----------------
    Vector3 GetRandomPointOnGround()
    {
        Renderer rend = gameGround.GetComponent<Renderer>();

        if (rend == null)
            return gameGround.position;

        Bounds bounds = rend.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float z = Random.Range(bounds.min.z, bounds.max.z);

        return new Vector3(x, bounds.max.y, z);
    }

    // ---------------- ENEMY TYPE ----------------
    GameObject GetRandomEnemy()
    {
        int r = Random.Range(0, 2);

        return r switch
        {
            0 => meleeEnemy,
            _ => shooterEnemy
        };
    }

    // ---------------- CLEANUP ----------------
    void CleanupDeadEnemies()
    {
        spawnedEnemies.RemoveAll(e => e == null);
    }

    void CleanupDeadMedkits()
    {
        spawnedMedkits.RemoveAll(m => m == null);
    }
}