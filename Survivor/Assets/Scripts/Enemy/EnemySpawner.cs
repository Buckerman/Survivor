using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using QuangDM.Common;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<Enemy> enemyPrefabs;
    [SerializeField] private float spawnInterval = 0.5f;
    [SerializeField] private Transform playerTransform;

    [SerializeField] private float planeSize = 4f;
    [SerializeField] private float maxSampleDistance = 5f;
    [SerializeField] private float spawnOffset = 1f;

    [SerializeField] private int initialEnemiesPerWave = 10;
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

        Observer.Instance.AddObserver("WaveCompleted", WaveCompleted);
    }

    private void WaveCompleted(object data)
    {
        _currentWave = (int)data;
        _enemiesSpawnedThisWave = 0;
        _totalEnemiesNextWave = initialEnemiesPerWave + (enemyIncrementPerWave * _currentWave);

        Observer.Instance.Notify("CurrentWaveLevel");
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
        Enemy enemy = _enemyPool.GetEnemy();
        Vector3 spawnPosition = GetRandomPositionOnGround();
        if (spawnPosition != Vector3.zero)
        {
            enemy.transform.position = spawnPosition + Vector3.up * spawnOffset;
            enemy.Initialize(playerTransform, _enemyPool);
            enemy.NavMeshAgent.enabled = true;
        }
    }

    private Vector3 GetRandomPositionOnGround()
    {
        Vector3 randomPosition = new Vector3(
            Random.Range(-planeSize * 10f / 2f, planeSize * 10f / 2f),
            0f,
            Random.Range(-planeSize * 10f / 2f, planeSize * 10f / 2f)
        );

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPosition, out hit, maxSampleDistance, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return Vector3.zero;
    }
}
