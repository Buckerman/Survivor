using System.Collections.Generic;
using UnityEngine;

public class BulletPool
{
    private Bullet bulletPrefab;
    private int poolSize;
    private Queue<Bullet> pool;

    public BulletPool(Bullet bulletPrefab, int poolSize)
    {
        this.bulletPrefab = bulletPrefab;
        this.poolSize = poolSize;
        pool = new Queue<Bullet>();

        for (int i = 0; i < poolSize; i++)
        {
            Bullet bullet = Object.Instantiate(bulletPrefab);
            bullet.gameObject.SetActive(false);
            pool.Enqueue(bullet);
        }
    }

    public Bullet GetBullet()
    {
        Bullet bullet;

        if (pool.Count > 0)
        {
            bullet = pool.Dequeue();
            if (bullet != null && bullet.gameObject != null)
            {
                bullet.gameObject.SetActive(true);
                return bullet;
            }
            else
            {
                return Object.Instantiate(bulletPrefab);
            }
        }
        else
        {
            return Object.Instantiate(bulletPrefab);
        }
    }

    public void ReturnBullet(Bullet bullet)
    {
        if (bullet != null && bullet.gameObject != null)
        {
            bullet.gameObject.SetActive(false);
            pool.Enqueue(bullet);
        }
    }
}
