using System.Collections.Generic;
using UnityEngine;

public class ARPlaneSpawner : MonoBehaviour
{
    [Header("AR Plane Root")]
    public Transform arPlane; // ONLY reference we keep

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

    [Header("Center Objective")]
    public GameObject centerPrefab;
    private GameObject spawnedCenterObject;

    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private List<GameObject> spawnedMedkits = new List<GameObject>();

    private bool bossSpawned = false;

    void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 2f, spawnInterval);
        InvokeRepeating(nameof(SpawnMedkit), 3f, medkitSpawnInterval);

        Invoke(nameof(SpawnBoss), 30f);

        SpawnCenterObject();
    }

    void Update()
    {
        LockCenterObject();
    }

    // ---------------- CENTER OBJECT ----------------

    void SpawnCenterObject()
    {
        if (centerPrefab == null || arPlane == null)
            return;

        if (spawnedCenterObject != null)
            return;

        spawnedCenterObject = Instantiate(
            centerPrefab,
            GetCenterPosition(),
            Quaternion.identity
        );
    }

    void LockCenterObject()
    {
        if (spawnedCenterObject == null || arPlane == null)
            return;

        spawnedCenterObject.transform.position = GetCenterPosition();
    }

    Vector3 GetCenterPosition()
    {
        return new Vector3(
    arPlane.position.x,
    10.97f,
    arPlane.position.z

        );
    }

    // ---------------- ENEMIES ----------------

    void SpawnEnemy()
    {
        if (bossSpawned) return;

        CleanupDeadEnemies();

        if (spawnedEnemies.Count >= maxEnemies) return;

        Vector3 spawnPosition = GetRandomPointOnPlane();

        GameObject enemyPrefab = GetRandomEnemy();

        if (enemyPrefab == meleeEnemy)
            spawnPosition.y += meleeHeight;
        else if (enemyPrefab == shooterEnemy)
            spawnPosition.y += shooterHeight;

        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        spawnedEnemies.Add(enemy);
    }

    // ---------------- MEDKIT ----------------

    void SpawnMedkit()
    {
        if (medkitPrefab == null || arPlane == null)
            return;

        CleanupDeadMedkits();

        if (spawnedMedkits.Count >= maxMedkits)
            return;

        Vector2 randomCircle = Random.insideUnitCircle * medkitSpawnRadius;

        Vector3 spawnPosition = new Vector3(
            arPlane.position.x + randomCircle.x,
            arPlane.position.y + medkitHeight,
            arPlane.position.z + randomCircle.y
        );

        GameObject medkit = Instantiate(medkitPrefab, spawnPosition, Quaternion.identity);
        spawnedMedkits.Add(medkit);
    }

    // ---------------- BOSS ----------------

    void SpawnBoss()
    {
        if (bossSpawned) return;

        bossSpawned = true;

        Vector3 spawnPosition = GetRandomPointOnPlane();
        spawnPosition.y += bossHeight;

        GameObject boss = Instantiate(bossEnemy, spawnPosition, Quaternion.identity);
        spawnedEnemies.Add(boss);
    }

    // ---------------- RANDOM POINT ----------------

    Vector3 GetRandomPointOnPlane()
    {
        Vector2 offset = Random.insideUnitCircle * 3f;

        return new Vector3(
            arPlane.position.x + offset.x,
            arPlane.position.y,
            arPlane.position.z + offset.y
        );
    }

    // ---------------- ENEMY TYPE ----------------

    GameObject GetRandomEnemy()
    {
        return Random.Range(0, 2) == 0 ? meleeEnemy : shooterEnemy;
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