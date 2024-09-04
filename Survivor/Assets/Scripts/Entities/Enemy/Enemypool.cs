using System.Collections.Generic;
using UnityEngine;

public class EnemyPool
{
    private List<EnemyController> enemyPrefabs;
    private int poolSize;
    private Queue<EnemyController> pool;

    public EnemyPool(List<EnemyController> enemyPrefabs, int poolSize)
    {
        this.enemyPrefabs = enemyPrefabs;
        this.poolSize = poolSize;
        pool = new Queue<EnemyController>();

        // Initialize pool with random enemy prefabs
        for (int i = 0; i < poolSize; i++)
        {
            EnemyController enemy = InstantiateRandomEnemyPrefab();
            enemy.gameObject.SetActive(false);
            pool.Enqueue(enemy);
        }
    }

    private EnemyController InstantiateRandomEnemyPrefab()
    {
        // Select a random prefab from the list
        int randomIndex = Random.Range(0, enemyPrefabs.Count);
        EnemyController randomPrefab = enemyPrefabs[randomIndex];
        return Object.Instantiate(randomPrefab);
    }

    public EnemyController GetEnemy()
    {
        EnemyController enemy;
        if (pool.Count > 0)
        {
            enemy = pool.Dequeue();
            enemy.gameObject.SetActive(true);
            return enemy;
        }
        else
        {
            return InstantiateRandomEnemyPrefab();
        }
    }

    public void ReturnEnemy(EnemyController enemy)
    {
        enemy.gameObject.SetActive(false);
        pool.Enqueue(enemy);
    }

    //moze kiedys sie przyda
    public void ResetPool(List<EnemyController> newEnemyPrefabs, int newSize)
    {
        // Clear current pool
        pool.Clear();
        enemyPrefabs = newEnemyPrefabs;
        poolSize = newSize;

        // Refill pool with new prefabs
        for (int i = 0; i < poolSize; i++)
        {
            EnemyController enemy = InstantiateRandomEnemyPrefab();
            enemy.gameObject.SetActive(false);
            pool.Enqueue(enemy);
        }
    }
}
