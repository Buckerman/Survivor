using System.Collections.Generic;
using UnityEngine;

public class EnemyPool
{
    private Enemy enemyPrefab;
    private int poolSize;
    private Queue<Enemy> pool;

    public EnemyPool(Enemy enemyPrefab, int poolSize)
    {
        this.enemyPrefab = enemyPrefab;
        this.poolSize = poolSize;
        pool = new Queue<Enemy>();

        for (int i = 0; i < poolSize; i++)
        {
            Enemy enemy = Object.Instantiate(enemyPrefab);
            enemy.gameObject.SetActive(false);
            pool.Enqueue(enemy);
        }
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
            Enemy enemy = Object.Instantiate(enemyPrefab);
            return enemy;
        }
    }

    public void ReturnEnemy(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
        pool.Enqueue(enemy);
    }
}
