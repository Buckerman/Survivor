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
    }
    private void DeactivateTrail()
    {
        _trailRenderer.enabled = false;
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

        currentSwordTrigger.ClearEnemiesInRange();
    }
}
