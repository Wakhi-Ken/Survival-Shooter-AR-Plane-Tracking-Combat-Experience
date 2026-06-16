using UnityEngine;

public class BodyRotation : MonoBehaviour
{
    public Transform arCamera;

    void Update()
    {
        Vector3 euler = transform.eulerAngles;

        euler.y = arCamera.eulerAngles.y;

        transform.eulerAngles = euler;
    }
}