using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARPlaneSpawner : MonoBehaviour
{
    public ARPlaneManager planeManager;

    public GameObject meleeEnemy;
    public GameObject shooterEnemy;
    public GameObject bossEnemy;

    public float spawnInterval = 5f;
    public int maxEnemies = 5;

    private List<GameObject> spawnedEnemies = new List<GameObject>();

    private bool bossSpawned = false;

    void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 2f, spawnInterval);

        // 👑 Boss spawn after 30 seconds
        Invoke(nameof(SpawnBoss), 30f);
    }

    void SpawnEnemy()
    {
        if (bossSpawned) return; // optional: stop normal spawns after boss

        List<ARPlane> planes = new List<ARPlane>();

        foreach (var plane in planeManager.trackables)
        {
            planes.Add(plane);
        }

        if (planes.Count == 0)
            return;

        if (spawnedEnemies.Count >= maxEnemies)
            return;

        ARPlane randomPlane =
            planes[Random.Range(0, planes.Count)];

        Vector3 spawnPosition =
            GetRandomPointOnPlane(randomPlane);

        GameObject enemyPrefab =
            GetRandomEnemy();

        GameObject enemy =
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        spawnedEnemies.Add(enemy);
    }

    void SpawnBoss()
    {
        if (bossSpawned) return;

        bossSpawned = true;

        Vector3 spawnPosition = GetSpawnInFrontOfCamera();

        GameObject boss =
            Instantiate(bossEnemy, spawnPosition, Quaternion.identity);

        spawnedEnemies.Add(boss);
    }

    Vector3 GetSpawnInFrontOfCamera()
    {
        Camera cam = Camera.main;

        Vector3 forward = cam.transform.forward;
        forward.y = 0;
        forward.Normalize();

        return cam.transform.position + forward * 3f;
    }

    Vector3 GetRandomPointOnPlane(ARPlane plane)
    {
        Vector2 size = plane.size;

        float x = Random.Range(-size.x / 2f, size.x / 2f);
        float z = Random.Range(-size.y / 2f, size.y / 2f);

        Vector3 localPoint =
            plane.center +
            plane.transform.right * x +
            plane.transform.forward * z;

        localPoint.y += 0.05f;

        return localPoint;
    }

    GameObject GetRandomEnemy()
    {
        int r = Random.Range(0, 2); // ONLY melee + shooter

        return r switch
        {
            0 => meleeEnemy,
            _ => shooterEnemy
        };
    }
}