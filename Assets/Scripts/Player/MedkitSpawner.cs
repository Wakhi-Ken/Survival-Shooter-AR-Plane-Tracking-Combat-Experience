using UnityEngine;

public class MedkitSpawner : MonoBehaviour
{
    public GameObject medkitPrefab;
    public float interval = 20f;

    float timer;

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            Instantiate(medkitPrefab,
                transform.position + Random.insideUnitSphere * 2,
                Quaternion.identity);

            timer = interval;
        }
    }
}