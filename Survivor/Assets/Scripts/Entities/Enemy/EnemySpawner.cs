using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using QuangDM.Common;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Spawn Settings")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnInterval = 0.3f;

    [SerializeField] private float spawnDistance = 10f;
    [SerializeField] private float maxSampleDistance = 2f;

    private float _timeSinceLastSpawn;
    private int _currentWave = 1;

    private void Start()
    {
        Observer.Instance.AddObserver(EventName.WaveCompleted, WaveCompleted);
    }

    private void WaveCompleted(object data)
    {
        _currentWave = (int)data;

        Observer.Instance.Notify(EventName.CurrentWaveLevel);
    }

    private void Update()
    {
        _timeSinceLastSpawn += Time.deltaTime;

        if (_timeSinceLastSpawn >= spawnInterval)
        {
            SpawnEnemy();
            _timeSinceLastSpawn = 0f;
        }
    }

    private void SpawnEnemy()
    {
        Vector3 spawnPosition = GetRandomPositionOnGround();
        if (spawnPosition != Vector3.zero)
        {
            GameObject enemyObject = ObjectPooling.Instance.GetObject(enemyPrefab);
            EnemyController enemy = enemyObject.GetComponent<EnemyController>();

            enemy.transform.position = spawnPosition;
            enemy.Initialize(Player.Instance.transform);
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

        Vector3 randomPosition = playerPosition + randomDirection * spawnDistance;

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
