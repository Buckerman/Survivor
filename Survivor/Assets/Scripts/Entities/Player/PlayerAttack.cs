using QuangDM.Common;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackDamage = 12f;

    [Header("Left Hand Target")]
    [SerializeField] private ChainIKConstraint leftHandIK;
    [SerializeField] private Transform leftHandTarget;

    private Animator animator;
    private SwordTrigger currentSwordTrigger;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        Observer.Instance.AddObserver("Slash", Slash);
    }

    private void FixedUpdate()
    {
        // Change IK weight
    }

    private void Slash(object data)
    {
        currentSwordTrigger = (SwordTrigger)data;
        animator.Play("PlayerAttack");
    }

    private void DealDamage()
    {
        if (currentSwordTrigger == null) return;

        List<EnemyHealth> enemiesInRange = currentSwordTrigger.GetEnemiesInRange();

        foreach (EnemyHealth enemy in enemiesInRange)
        {
            if (enemy != null && enemy.gameObject.activeInHierarchy)
            {
                enemy.TakeDamage(attackDamage);
            }
        }

        currentSwordTrigger.ClearEnemiesInRange();
    }
}
