using UnityEngine;

public class UpperBodyAim : MonoBehaviour
{
    public Transform arCamera;

    void LateUpdate()
    {
        Vector3 rot = transform.localEulerAngles;

        float pitch = arCamera.eulerAngles.x;

        if (pitch > 180)
            pitch -= 360;

        rot.x = Mathf.Clamp(
            pitch,
            -45f,
            45f
        );

        transform.localEulerAngles = rot;
    }
}