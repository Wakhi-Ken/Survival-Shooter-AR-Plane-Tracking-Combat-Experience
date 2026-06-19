using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform arCamera;

    void Update()
    {
        if (arCamera == null) return;

        Vector3 direction =
            arCamera.position - transform.position;

        direction.y = 0f; // keep character upright

        if (direction.sqrMagnitude > 0.01f)
        {
            transform.rotation =
                Quaternion.LookRotation(direction);
        }
    }
}