using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public FixedJoystick joystick;
    public Transform cameraTransform;

    public float moveSpeed = 2f;
    public float rotationSpeed = 8f;

    private CharacterController controller;
    private Animator animator;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (joystick == null || cameraTransform == null)
            return;

        // Get camera directions
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // Ignore vertical tilt
        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        // Camera-relative movement
        Vector3 move =
            forward * joystick.Vertical +
            right * joystick.Horizontal;

        if (move.magnitude > 0.1f)
        {
            controller.Move(move.normalized * moveSpeed * Time.deltaTime);

            Quaternion targetRotation = Quaternion.LookRotation(move);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime);

            if (animator != null)
                animator.SetBool("Walking", true);
        }
        else
        {
            if (animator != null)
                animator.SetBool("Walking", false);
        }
    }
}