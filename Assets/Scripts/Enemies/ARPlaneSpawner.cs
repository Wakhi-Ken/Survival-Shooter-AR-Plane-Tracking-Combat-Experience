using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARPlaneSpawner : MonoBehaviour
{
    [Header("AR")]
    [SerializeField] private ARPlaneManager planeManager;
    [SerializeField] private ARRaycastManager raycastManager;

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
    public float spawnInterval = 1.5f;

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
    private bool worldSpawned = false;

    private void Start()
    {
        if (planeManager != null)
        {
            planeManager.planesChanged += OnPlanesChanged;
        }
        else
        {
            Debug.LogError("ARPlaneManager is not assigned.");
        }
    }

    private void Update()
    {
        if (!worldSpawned)
            return;

        LockCenterObject();
    }

    private void OnDestroy()
    {
        if (planeManager != null)
        {
            planeManager.planesChanged -= OnPlanesChanged;
        }
    }

    // PLANE DETECTION


    private void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        if (worldSpawned)
            return;

        if (args.added.Count == 0)
            return;

        ARPlane detectedPlane = args.added[0];

        arPlane = detectedPlane.transform;

        SpawnGameWorld();
    }

    private void SpawnGameWorld()
    {
        worldSpawned = true;

        SpawnCenterObject();

        InvokeRepeating(nameof(SpawnEnemy), 2f, spawnInterval);
        InvokeRepeating(nameof(SpawnMedkit), 3f, medkitSpawnInterval);
        Invoke(nameof(SpawnBoss), 30f);

        LockPlaneDetection();

        Debug.Log("Game world spawned.");
    }

    private void LockPlaneDetection()
    {
        if (planeManager == null)
            return;

        foreach (ARPlane plane in planeManager.trackables)
        {
            plane.gameObject.SetActive(false);
        }

        planeManager.enabled = false;

        if (raycastManager != null)
            raycastManager.enabled = false;
    }


    // CENTER OBJECT
 

    private void SpawnCenterObject()
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

    private void LockCenterObject()
    {
        if (spawnedCenterObject == null || arPlane == null)
            return;

        spawnedCenterObject.transform.position = GetCenterPosition();
    }

    private Vector3 GetCenterPosition()
    {
        return new Vector3(
            arPlane.position.x,
            10.97f,
            arPlane.position.z
        );
    }

    // ENEMIES


    private void SpawnEnemy()
    {
        if (!worldSpawned)
            return;

        if (GameManager.Instance != null &&
            GameManager.Instance.CurrentState != GameState.Playing)
            return;

        if (arPlane == null)
            return;

        Vector3 spawnPosition = GetRandomPointOnPlane();
        GameObject enemyPrefab = GetRandomEnemy();

        if (enemyPrefab == meleeEnemy)
            spawnPosition.y += meleeHeight;
        else
            spawnPosition.y += shooterHeight;

        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }


    // MEDKITS


    private void SpawnMedkit()
    {
        if (!worldSpawned)
            return;

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


    // BOSS


    private void SpawnBoss()
    {
        if (!worldSpawned || bossSpawned)
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

    
    // RANDOM
    

    private Vector3 GetRandomPointOnPlane()
    {
        Vector2 offset = Random.insideUnitCircle * 3f;

        return new Vector3(
            arPlane.position.x + offset.x,
            arPlane.position.y,
            arPlane.position.z + offset.y
        );
    }

    private GameObject GetRandomEnemy()
    {
        return Random.Range(0, 2) == 0
            ? meleeEnemy
            : shooterEnemy;
    }
}