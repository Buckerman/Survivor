using System.Collections.Generic;
using UnityEngine;

public class EnemyPool
{
    private List<Enemy> enemyPrefabs;
    private int poolSize;
    private Queue<Enemy> pool;

    public EnemyPool(List<Enemy> enemyPrefabs, int poolSize)
    {
        this.enemyPrefabs = enemyPrefabs;
        this.poolSize = poolSize;
        pool = new Queue<Enemy>();

        // Initialize pool with random enemy prefabs
        for (int i = 0; i < poolSize; i++)
        {
            Enemy enemy = InstantiateRandomEnemyPrefab();
            enemy.gameObject.SetActive(false);
            pool.Enqueue(enemy);
        }
    }

    private Enemy InstantiateRandomEnemyPrefab()
    {
        // Select a random prefab from the list
        int randomIndex = Random.Range(0, enemyPrefabs.Count);
        Enemy randomPrefab = enemyPrefabs[randomIndex];
        return Object.Instantiate(randomPrefab);
    }

    public Enemy GetEnemy()
    {
        if (pool.Count > 0)
        {
            Enemy enemy = pool.Dequeue();
            enemy.gameObject.SetActive(true);
            return enemy;
        }
        else
        {
            return InstantiateRandomEnemyPrefab();
        }
    }

    public void ReturnEnemy(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
        pool.Enqueue(enemy);
    }

    //moze kiedys sie przyda
    public void ResetPool(List<Enemy> newEnemyPrefabs, int newSize)
    {
        // Clear current pool
        pool.Clear();
        enemyPrefabs = newEnemyPrefabs;
        poolSize = newSize;

        // Refill pool with new prefabs
        for (int i = 0; i < poolSize; i++)
        {
            Enemy enemy = InstantiateRandomEnemyPrefab();
            enemy.gameObject.SetActive(false);
            pool.Enqueue(enemy);
        }
    }
}
