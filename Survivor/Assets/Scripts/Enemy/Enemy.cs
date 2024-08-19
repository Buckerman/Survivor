using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using QuangDM.Common;
using System;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform _player;
    private EnemyPool _enemyPool;
    private Animator _animator;

    public NavMeshAgent NavMeshAgent => agent;

    [SerializeField] private float speed = 2f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private float fallbackDistance = 10f; // Distance below player to fallback to

    private IEnemyState _currentState;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        agent.speed = speed;
    }

    private void Start()
    {
        Observer.Instance.AddObserver("DisableAllEnemies", DisableAllEnemies);
    }

    public void Initialize(Transform playerTransform, EnemyPool pool)
    {
        _player = playerTransform;
        _enemyPool = pool;

        agent.enabled = false;
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        if (_player != null)
        {
            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(_player.position, path);
            if (path.status == NavMeshPathStatus.PathComplete)
            {
                agent.SetDestination(_player.position);

                float distanceToPlayer = Vector3.Distance(transform.position, _player.position);
                if (distanceToPlayer <= attackRange)
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
                // Calculate fallback position directly below the player
                Vector3 fallbackPosition = GetFallbackPosition();
                if (fallbackPosition != Vector3.zero)
                {
                    agent.SetDestination(fallbackPosition);
                }
            }
        }
    }

    private void AttackPlayer()
    {
        _player.GetComponent<PlayerBehaviour>().TakeDamage(attackDamage);
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
        OnDisable();
    }
    private void OnDisable()
    {
        if (_enemyPool != null)
        {
            _enemyPool.ReturnEnemy(this);
        }
    }
}
