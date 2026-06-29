using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [Header("Animator")]
    private Animator animator;

    [Header("Weapon Setup")]
    public GameObject weaponPrefab;
    public Transform handSocket;

    [Header("Weapon Offset")]
    public Vector3 weaponPositionOffset;
    public Vector3 weaponRotationOffset;

    private GameObject currentWeapon;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        AttachWeapon();
    }

    void AttachWeapon()
    {
        if (weaponPrefab == null || handSocket == null)
            return;

        currentWeapon = Instantiate(weaponPrefab, handSocket);

        currentWeapon.transform.localPosition = weaponPositionOffset;
        currentWeapon.transform.localRotation = Quaternion.Euler(weaponRotationOffset);
    }

    public void PlayShoot()
    {
        if (animator != null)
            animator.SetTrigger("Shoot");
    }

    public void PlayReload()
    {
        if (animator != null)
            animator.SetTrigger("Reload");
    }

    
    public void PlayDie()
    {
        if (animator != null)
        {
            animator.ResetTrigger("Shoot");
            animator.ResetTrigger("Reload");
            animator.SetTrigger("Die");
        }

        
        if (currentWeapon != null)
        {
            Destroy(currentWeapon);
        }
    }
}