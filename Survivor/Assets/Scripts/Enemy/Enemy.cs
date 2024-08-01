using UnityEngine;

public class Enemy : MonoBehaviour
{
    private EnemyPool enemyPool;
    private Transform _player;
    [SerializeField] private float speed = 2f;

    public void Initialize(EnemyPool pool, Transform playerTransform)
    {
        enemyPool = pool;
        _player = playerTransform;
    }

    private void Update()
    {
        if (_player != null)
        {
            Vector3 direction = (_player.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
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
