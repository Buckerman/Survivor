using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform _player;
    private EnemyPool enemyPool;

    [SerializeField] private float speed = 2f;

    [SerializeField] private float attackRange = 2f;
    [SerializeField] private int attackDamage = 10;
    private float attackCooldown = 2f;
    private float lastAttackTime;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
    }

    public void Initialize(Transform playerTransform, EnemyPool pool)
    {
        _player = playerTransform;
        enemyPool = pool;
    }

    private void Update()
    {
        if (_player != null)
        {
            agent.SetDestination(_player.position);

            float distanceToPlayer = Vector3.Distance(transform.position, _player.position);
            if (distanceToPlayer <= attackRange && Time.time >= lastAttackTime + attackCooldown)
            {
                AttackPlayer();
                lastAttackTime = Time.time;
            }
        }
    }

    private void AttackPlayer()
    {
        agent.isStopped = true;
        PlayerBehaviour player = _player.GetComponent<PlayerBehaviour>();
        if (player != null)
        {
            player.TakeDamage(attackDamage);
        }
    }

    private void OnDisable()
    {
        if (enemyPool != null)
        {
            enemyPool.ReturnEnemy(this);
        }
    }
}
