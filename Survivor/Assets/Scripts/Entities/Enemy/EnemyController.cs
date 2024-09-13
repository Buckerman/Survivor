using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using QuangDM.Common;
using System;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] private float enemySpeed = 2f;
    [SerializeField] private float enemyAttackRange = 1.8f;
    [SerializeField] private int enemyAttackDamage = 5;
    [SerializeField] private float fallbackDistance = 10f; // Distance below player to fallback to
    [SerializeField] private float maxDistanceFromPlayer = 15f;

    private NavMeshAgent agent;
    private Transform _player;
    private Animator _animator;
    public NavMeshAgent NavMeshAgent => agent;

    private IEnemyState _currentState;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        agent.speed = enemySpeed;
    }

    public void Initialize(Transform playerTransform)
    {
        _player = playerTransform;

        Observer.Instance.AddObserver(EventName.DisableAllEnemies, DisableAllEnemies);
        agent.enabled = false;
    }

    private void FixedUpdate()
    {
        Chase();
    }

    private void Chase()
    {
        if (_player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, _player.position);
            if (distanceToPlayer > maxDistanceFromPlayer)
            {
                OnDisable();
                return;
            }

            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(_player.position, path);
            if (path.status == NavMeshPathStatus.PathComplete)
            {
                agent.SetDestination(_player.position);

                //check if player is in enemy range and visible angle
                if (distanceToPlayer <= enemyAttackRange && Mathf.Abs(transform.eulerAngles.x) < 1f && Mathf.Abs(transform.eulerAngles.z) < 1f)
                {
                    agent.isStopped = true;
                    SetState(new AttackState());
                }
                else
                {
                    agent.isStopped = false;
                    SetState(new WalkState());
                }
            }
            else
            {
                Vector3 fallbackPosition = GetFallbackPosition();
                if (fallbackPosition != Vector3.zero)
                {
                    agent.SetDestination(fallbackPosition);
                }
            }
        }
    }

    //called in animator
    private void AttackPlayer()
    {
        _player.GetComponent<PlayerHealth>().TakeDamage(enemyAttackDamage);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(enemyAttackDamage);
            }
        }
    }

    private Vector3 GetFallbackPosition()
    {
        Vector3 playerPosition = _player.position;
        Vector3 fallbackPosition = playerPosition - new Vector3(0, fallbackDistance, 0);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(fallbackPosition, out hit, 1.0f, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return Vector3.zero;
    }

    public void SetState(IEnemyState newState)
    {
        if (_currentState != null)
        {
            _currentState.Exit();
        }
        _currentState = newState;
        _currentState.Enter(this);
    }

    public void SetAnimation(string parameter, bool state)
    {
        _animator.SetBool(parameter, state);
    }

    private void DisableAllEnemies(object data)
    {
        if (this != null)
        {
            OnDisable();
        }
    }

    private void OnDisable()
    {
        Invoke(nameof(RemoveObserver), 0f);
        this.gameObject.SetActive(false);
    }

    private void RemoveObserver()
    {
        if (Observer.Instance != null)
        {
            Observer.Instance.RemoveObserver(EventName.DisableAllEnemies, DisableAllEnemies);
        }
    }

}
