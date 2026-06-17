using UnityEngine;

public class FollowCameraBody : MonoBehaviour
{
    public Transform arCamera;

    void Update()
    {
        transform.position = arCamera.position;
    }
}