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
    //public Bullet GetBullet()
    //{
    //    Bullet bullet;

    //    if (pool.Count > 0)
    //    {
    //        bullet = pool.Dequeue();
    //        if (bullet != null && bullet.gameObject != null)
    //        {
    //            bullet.gameObject.SetActive(true);
    //        }
    //        else
    //        {
    //            bullet = Object.Instantiate(bulletPrefab);
    //        }
    //    }
    //    else
    //    {
    //        bullet = Object.Instantiate(bulletPrefab);
    //    }

    //    return bullet;
    //}
    public Bullet GetBullet() //shortened version
    {
        Bullet bullet = (pool.Count > 0) ? pool.Dequeue() : null;

        if (bullet == null || bullet.gameObject == null)
        {
            bullet = Object.Instantiate(bulletPrefab);
        }

        bullet.gameObject.SetActive(true);
        return bullet;
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
