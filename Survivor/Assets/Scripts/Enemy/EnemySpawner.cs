using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private int poolSize = 10;
    [SerializeField] private float spawnInterval = 0.5f;
    [SerializeField] private Transform playerTransform;

    [SerializeField] private float planeSize = 4f;
    [SerializeField] private float maxSampleDistance = 5f;
    [SerializeField] private float spawnOffset = 1f;

    private float timeSinceLastSpawn;
    private EnemyPool _enemyPool;

    private void Start()
    {
        _enemyPool = new EnemyPool(enemyPrefab, poolSize);
    }

    private void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= spawnInterval)
        {
            SpawnEnemy();
            timeSinceLastSpawn = 0f;
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
            0f, // 1 size to 10 kratek
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
