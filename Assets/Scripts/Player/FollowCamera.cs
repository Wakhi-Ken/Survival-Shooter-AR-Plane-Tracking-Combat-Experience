using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private Transform arCamera;

    void Start()
    {
        if (Camera.main != null)
        {
            arCamera = Camera.main.transform;
        }
    }

    void Update()
    {
        if (arCamera == null) return;

        Vector3 direction =
            arCamera.position - transform.position;

        direction.y = 0f; // keep upright

        if (direction.sqrMagnitude > 0.01f)
        {
            transform.rotation =
                Quaternion.LookRotation(direction);
        }
    }
}