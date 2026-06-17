using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    public GameObject boss;
    public float spawnTime = 120f;

    float timer;

    bool spawned;

    void Update()
    {
        if (spawned) return;

        timer += Time.deltaTime;

        if (timer >= spawnTime)
        {
            Instantiate(boss, transform.position, Quaternion.identity);
            spawned = true;
        }
    }
}