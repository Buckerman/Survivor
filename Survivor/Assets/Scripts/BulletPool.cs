using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private int poolSize = 10;
    private Queue<Bullet> pool;

    public static BulletPool Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        pool = new Queue<Bullet>();
        for (int i = 0; i < poolSize; i++)
        {
            Bullet bullet = Instantiate(bulletPrefab);
            bullet.gameObject.SetActive(false);
            pool.Enqueue(bullet);
        }
    }

    public Bullet GetBullet()
    {
        if (pool.Count > 0)
        {
            Bullet bullet = pool.Dequeue();
            if (bullet != null && bullet.gameObject != null)
            {
                bullet.gameObject.SetActive(true);
                return bullet;
            }
            else
            {
                return Instantiate(bulletPrefab);
            }
        }
        else
        {
            return Instantiate(bulletPrefab);
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
