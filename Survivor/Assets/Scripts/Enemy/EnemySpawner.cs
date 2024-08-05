using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private int poolSize = 10;
    [SerializeField] private float spawnInterval = 0.5f;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private float planeSize = 4f;
    [SerializeField] private float raycastHeight = 10f;
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
        }
    }

    private Vector3 GetRandomPositionOnGround()
    {
        Vector3 randomPosition = new Vector3(
            Random.Range(-planeSize * 10f / 2f, planeSize * 10f / 2f),
            raycastHeight, // 1 size to 10 kratek
            Random.Range(-planeSize * 10f / 2f, planeSize * 10f / 2f)
        );

        Ray ray = new Ray(randomPosition, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            return hit.point;
        }
        return Vector3.zero;
    }
}
