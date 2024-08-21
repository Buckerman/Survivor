using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackDamage = 12f;

    [Header("Left Hand Target")]
    [SerializeField] private ChainIKConstraint leftHandIK;
    [SerializeField] private Transform leftHandTarget;

    private Animator animator;
    private Collider leftHandCollider;

    private void Start()
    {
        animator = GetComponent<Animator>();
        leftHandCollider = leftHandTarget.GetComponent<Collider>();
    }

    private void FixedUpdate()
    {
        if (animator.GetLayerWeight(1) > 0f)
        {
            leftHandTarget.position = transform.position + transform.forward + Vector3.up * 1f;
        }
        else
        {
            leftHandIK.weight = 0.3f;
        }
    }
}
