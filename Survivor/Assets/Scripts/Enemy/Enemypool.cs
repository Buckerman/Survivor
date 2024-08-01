using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private int poolSize = 10;
    private Queue<Enemy> pool;

    public static EnemyPool Instance { get; private set; }

    private void Awake()
    {
        pool = new Queue<Enemy>();

        for (int i = 0; i < poolSize; i++)
        {
            Enemy enemy = Instantiate(enemyPrefab);
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
            Enemy enemy = Instantiate(enemyPrefab);
            return enemy;
        }
    }

    public void ReturnEnemy(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
        pool.Enqueue(enemy);
    }
}
