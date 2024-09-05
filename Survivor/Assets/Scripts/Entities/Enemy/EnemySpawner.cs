using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using QuangDM.Common;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<EnemyController> enemyPrefabs;
    [SerializeField] private float spawnInterval = 0.3f;

    [SerializeField] private float radius = 15f;
    [SerializeField] private float maxSampleDistance = 2f;

    [SerializeField] private int initialEnemiesPerWave = 50;
    [SerializeField] private int enemyIncrementPerWave = 5;

    private float _timeSinceLastSpawn;
    private int _currentWave = 1;
    private int _enemiesSpawnedThisWave = 0;
    private int _totalEnemiesNextWave;
    private EnemyPool _enemyPool;

    private void Start()
    {
        // Initialize the enemy pool with the first type of enemy in the list
        _enemyPool = new EnemyPool(enemyPrefabs, initialEnemiesPerWave);
        _totalEnemiesNextWave = initialEnemiesPerWave;

        Observer.Instance.AddObserver(EventName.WaveCompleted, WaveCompleted);
    }

    private void WaveCompleted(object data)
    {
        _currentWave = (int)data;
        _enemiesSpawnedThisWave = 0;
        _totalEnemiesNextWave = initialEnemiesPerWave + (enemyIncrementPerWave * _currentWave);

        Observer.Instance.Notify(EventName.CurrentWaveLevel);
    }

    private void Update()
    {
        _timeSinceLastSpawn += Time.deltaTime;

        if (_timeSinceLastSpawn >= spawnInterval && _enemiesSpawnedThisWave < _totalEnemiesNextWave)
        {
            SpawnEnemy();
            _timeSinceLastSpawn = 0f;
            _enemiesSpawnedThisWave++;
        }
    }

    private void SpawnEnemy()
    {
        EnemyController enemy = _enemyPool.GetEnemy();
        Vector3 spawnPosition = GetRandomPositionOnGround();
        if (spawnPosition != Vector3.zero)
        {
            enemy.transform.position = spawnPosition;
            enemy.Initialize(Player.Instance.transform, _enemyPool);
            enemy.NavMeshAgent.enabled = true;
            enemy.SetState(new WalkState());
        }
    }

    private Vector3 GetRandomPositionOnGround()
    {
        Vector3 playerPosition = Player.Instance.transform.position;
        Vector3 randomDirection = new Vector3(
            Random.Range(-1f, 1f),
            0f,
            Random.Range(-1f, 1f)
        ).normalized;

        float randomDistance = Random.Range(10f, radius);

        Vector3 randomPosition = playerPosition + randomDirection * randomDistance;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPosition, out hit, maxSampleDistance, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return Vector3.zero;
    }

    void OnDestroy()
    {
        Observer.Instance.RemoveObserver(EventName.WaveCompleted, WaveCompleted);
    }
}
