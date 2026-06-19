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

    void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 2f, spawnInterval);
    }

    void SpawnEnemy()
    {
        // ✔ FIX: convert trackables properly
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

    Vector3 GetRandomPointOnPlane(ARPlane plane)
    {
        Vector2 size = plane.size;

        float x = Random.Range(-size.x / 2f, size.x / 2f);
        float z = Random.Range(-size.y / 2f, size.y / 2f);

        Vector3 localPoint =
            plane.center +
            plane.transform.right * x +
            plane.transform.forward * z;

        localPoint.y += 0.05f; // small lift above surface

        return localPoint;
    }

    GameObject GetRandomEnemy()
    {
        int r = Random.Range(0, 3);

        return r switch
        {
            0 => meleeEnemy,
            1 => shooterEnemy,
            _ => bossEnemy
        };
    }
}