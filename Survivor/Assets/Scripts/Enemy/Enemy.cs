using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform _player;
    private EnemyPool _enemyPool;

    [SerializeField] private float speed = 2f;

    [SerializeField] private float attackRange = 2f;
    [SerializeField] private int attackDamage = 10;
    private float attackCooldown = 2f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
    }

    public void Initialize(Transform playerTransform, EnemyPool pool)
    {
        _player = playerTransform;
        _enemyPool = pool;
    }

    private bool isAttacking = false;
    private void Update()
    {
        if (_player != null)
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
    }

    private IEnumerator AttackPlayer()
    {
        isAttacking = true;

        yield return new WaitForSeconds(attackCooldown);// delay for animation perform

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

    private void OnDisable()
    {
        if (_enemyPool != null)
        {
            _enemyPool.ReturnEnemy(this);
        }
    }
}
