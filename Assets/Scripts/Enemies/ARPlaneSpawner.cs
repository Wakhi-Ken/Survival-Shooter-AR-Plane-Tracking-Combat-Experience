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

    [Header("Spawn Settings")]
    public float spawnInterval = 5f;
    public int maxEnemies = 5;

    [Header("Spawn Heights")]
    public float meleeHeight = 1.2f;
    public float shooterHeight = 1.2f;
    public float bossHeight = 1.2f;

    private List<GameObject> spawnedEnemies = new List<GameObject>();

    private bool bossSpawned = false;

    void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 2f, spawnInterval);

        // Spawn boss after 30 seconds
        Invoke(nameof(SpawnBoss), 30f);
    }

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
        {
            spawnPosition.y += meleeHeight;
        }
        else if (enemyPrefab == shooterEnemy)
        {
            spawnPosition.y += shooterHeight;
        }

        GameObject enemy =
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        spawnedEnemies.Add(enemy);
    }

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

    Vector3 GetRandomPointOnGround()
    {
        Renderer rend = gameGround.GetComponent<Renderer>();

        if (rend == null)
        {
            return gameGround.position;
        }

        Bounds bounds = rend.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float z = Random.Range(bounds.min.z, bounds.max.z);

        return new Vector3(
            x,
            bounds.max.y,
            z
        );
    }

    GameObject GetRandomEnemy()
    {
        int r = Random.Range(0, 2);

        return r switch
        {
            0 => meleeEnemy,
            _ => shooterEnemy
        };
    }

    void CleanupDeadEnemies()
    {
        spawnedEnemies.RemoveAll(enemy => enemy == null);
    }
}