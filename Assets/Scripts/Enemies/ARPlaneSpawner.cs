using System.Collections.Generic;
using UnityEngine;

public class ARPlaneSpawner : MonoBehaviour
{
    [Header("AR Plane Root")]
    public Transform arPlane;

    [Header("Enemy Prefabs")]
    public GameObject meleeEnemy;
    public GameObject shooterEnemy;
    public GameObject bossEnemy;

    [Header("Medkit")]
    public GameObject medkitPrefab;
    public float medkitSpawnInterval = 12f;
    public float medkitSpawnRadius = 3f;
    public int maxMedkits = 3;

    [Header("Enemy Spawn Settings")]
    public float spawnInterval = 1.5f; // Faster spawning

    [Header("Spawn Heights")]
    public float meleeHeight = 1.2f;
    public float shooterHeight = 1.2f;
    public float bossHeight = 1.2f;
    public float medkitHeight = 1.0f;

    [Header("Center Objective")]
    public GameObject centerPrefab;

    private GameObject spawnedCenterObject;
    private readonly List<GameObject> spawnedMedkits = new();

    private bool bossSpawned = false;

    void Start()
    {
        SpawnCenterObject();

        InvokeRepeating(nameof(SpawnEnemy), 2f, spawnInterval);
        InvokeRepeating(nameof(SpawnMedkit), 3f, medkitSpawnInterval);
        Invoke(nameof(SpawnBoss), 30f);
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
        if (GameManager.Instance != null &&
            GameManager.Instance.CurrentState != GameState.Playing)
            return;

        if (arPlane == null)
            return;

        Vector3 spawnPosition = GetRandomPointOnPlane();
        GameObject enemyPrefab = GetRandomEnemy();

        if (enemyPrefab == meleeEnemy)
            spawnPosition.y += meleeHeight;
        else if (enemyPrefab == shooterEnemy)
            spawnPosition.y += shooterHeight;

        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }

    // ---------------- MEDKITS ----------------

    void SpawnMedkit()
    {
        if (GameManager.Instance != null &&
            GameManager.Instance.CurrentState != GameState.Playing)
            return;

        if (medkitPrefab == null || arPlane == null)
            return;

        spawnedMedkits.RemoveAll(item => item == null);

        if (spawnedMedkits.Count >= maxMedkits)
            return;

        Vector2 randomCircle = Random.insideUnitCircle * medkitSpawnRadius;

        Vector3 spawnPosition = new Vector3(
            arPlane.position.x + randomCircle.x,
            arPlane.position.y + medkitHeight,
            arPlane.position.z + randomCircle.y
        );

        GameObject medkit = Instantiate(
            medkitPrefab,
            spawnPosition,
            Quaternion.identity
        );

        spawnedMedkits.Add(medkit);
    }

    // ---------------- BOSS ----------------

    void SpawnBoss()
    {
        if (bossSpawned)
            return;

        if (GameManager.Instance != null &&
            GameManager.Instance.CurrentState != GameState.Playing)
            return;

        bossSpawned = true;

        Vector3 spawnPosition = GetRandomPointOnPlane();
        spawnPosition.y += bossHeight;

        Instantiate(
            bossEnemy,
            spawnPosition,
            Quaternion.identity
        );

        if (GameManager.Instance != null)
        {
            GameManager.Instance.ShowBossMessage(
                "Boss has spawned! Defeat it!",
                4f
            );
        }
    }

    // ---------------- RANDOM ----------------

    Vector3 GetRandomPointOnPlane()
    {
        Vector2 offset = Random.insideUnitCircle * 3f;

        return new Vector3(
            arPlane.position.x + offset.x,
            arPlane.position.y,
            arPlane.position.z + offset.y
        );
    }

    GameObject GetRandomEnemy()
    {
        return Random.Range(0, 2) == 0
            ? meleeEnemy
            : shooterEnemy;
    }
}