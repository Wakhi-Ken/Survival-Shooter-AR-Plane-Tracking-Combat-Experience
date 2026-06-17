using UnityEngine;

public class WeaponFollowSocket : MonoBehaviour
{
    [Header("Socket Reference")]
    public Transform weaponSocket;

    [Header("Weapon")]
    public Transform weapon;

    [Header("Smoothing")]
    public float positionSmooth = 20f;
    public float rotationSmooth = 20f;

    [Header("Offsets")]
    public Vector3 positionOffset;
    public Vector3 rotationOffset;

    void LateUpdate()
    {
        if (weaponSocket == null || weapon == null) return;

        // Apply offset relative to socket orientation
        Vector3 targetPosition =
            weaponSocket.position +
            weaponSocket.TransformDirection(positionOffset);

        Quaternion targetRotation =
            weaponSocket.rotation *
            Quaternion.Euler(rotationOffset);

        // Smooth position
        weapon.position = Vector3.Lerp(
            weapon.position,
            targetPosition,
            Time.deltaTime * positionSmooth
        );

        // Smooth rotation
        weapon.rotation = Quaternion.Slerp(
            weapon.rotation,
            targetRotation,
            Time.deltaTime * rotationSmooth
        );
    }
}