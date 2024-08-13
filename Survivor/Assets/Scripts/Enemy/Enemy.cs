using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform _player;
    private EnemyPool _enemyPool;

    public NavMeshAgent NavMeshAgent => agent;

    [SerializeField] private float speed = 2f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private int attackDamage = 10;
    private float attackCooldown = 2f;
    [SerializeField] private float fallbackDistance = 10f; // Distance below player to fallback to

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
    }

    public void Initialize(Transform playerTransform, EnemyPool pool)
    {
        _player = playerTransform;
        _enemyPool = pool;

        agent.enabled = false;
    }

    private bool isAttacking = false;

    private void Update()
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
                    if (!isAttacking)
                    {
                        StartCoroutine(AttackPlayer());
                    }
                }
                else
                {
                    agent.isStopped = false;
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

    private IEnumerator AttackPlayer()
    {
        isAttacking = true;
        yield return new WaitForSeconds(attackCooldown);

        if (_player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, _player.position);
            if (distanceToPlayer <= attackRange)
            {
                PlayerBehaviour player = _player.GetComponent<PlayerBehaviour>();
                if (player != null)
                {
                    player.TakeDamage(attackDamage);
                }
            }
        }
        isAttacking = false;
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

    private void OnDisable()
    {
        if (_enemyPool != null)
        {
            _enemyPool.ReturnEnemy(this);
        }
    }
}
