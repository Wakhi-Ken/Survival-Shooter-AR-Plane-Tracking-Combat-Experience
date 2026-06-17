using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public Animator animator;
    public Transform arCamera;

    Vector3 lastPos;

    void Start()
    {
        lastPos = arCamera.position;
    }

    void Update()
    {
        bool moving = Vector3.Distance(arCamera.position, lastPos) > 0.01f;

        animator.SetBool("IsWalking", moving);

        lastPos = arCamera.position;
    }

    public void ShootAnim()
    {
        animator.SetTrigger("Shoot");
    }
}