using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackDamage = 12f;

    [Header("Left Hand Target")]
    [SerializeField] private ChainIKConstraint leftHandIK;
    [SerializeField] private Transform leftHandTarget;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (animator.GetLayerWeight(1) > 0f) 
        {
            leftHandTarget.position = transform.position + transform.forward; 
            leftHandIK.weight = animator.GetLayerWeight(1);
        }
        else
        {
            leftHandIK.weight = 0.3f;
        }
    }
}
