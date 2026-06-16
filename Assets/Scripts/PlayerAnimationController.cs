using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public Transform arCamera;
    public Animator animator;

    Vector3 lastPosition;

    private void Start()
    {
        lastPosition = arCamera.position;
    }

    private void Update()
    {
        float distance =
            Vector3.Distance(
                arCamera.position,
                lastPosition
            );

        bool moving = distance > 0.01f;

        animator.SetBool(
            "IsWalking",
            moving
        );

        lastPosition = arCamera.position;
    }

    public void PlayShootAnimation()
    {
        animator.SetTrigger("Shoot");
    }
}