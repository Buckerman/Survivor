using QuangDM.Common;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerAttack : MonoBehaviour
{
    [Header("Left Hand Target")]
    [SerializeField] private ChainIKConstraint leftHandIK;
    [SerializeField] private Transform leftHandTarget;

    private Animator _animator;
    private TrailRenderer _trailRenderer;
    private SwordTrigger currentSwordTrigger;
    public bool isAttacking;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _trailRenderer = GetComponentInChildren<TrailRenderer>();
    }
    private void Start()
    {
        Observer.Instance.AddObserver(EventName.Slash, Slash);
    }

    private void FixedUpdate()
    {
        // Change IK weight
    }

    private void Slash(object data)
    {
        currentSwordTrigger = (SwordTrigger)data;
        _animator.Play("PlayerAttack");
    }

    private void ActivateTrail()
    {
        _trailRenderer.enabled = true;
        isAttacking = true;
    }
    private void DeactivateTrail()
    {
        _trailRenderer.enabled = false;
        isAttacking = false;
    }

    private void DealDamage()
    {
        if (currentSwordTrigger == null) return;

        List<EnemyHealth> enemiesInRange = currentSwordTrigger.GetEnemiesInRange();

        foreach (EnemyHealth enemy in enemiesInRange)
        {
            if (enemy != null && enemy.gameObject.activeInHierarchy)
            {
                enemy.TakeDamage(Player.Instance.attackDamage);
            }
        }
    }
}
