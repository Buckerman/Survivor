using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform _player;
    private EnemyPool enemyPool;

    [SerializeField] private float speed = 2f;

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
