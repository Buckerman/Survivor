using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private float shootInterval = 2f;
    [SerializeField] private Vector3 bulletSpawnOffset = new Vector3(0.5f, 0, 0.5f);
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private int bulletPoolSize = 10;

    private Transform closestEnemy;
    private float shootTimer;
    private BulletPool _bulletPool;

    private void Start()
    {
        _bulletPool = new BulletPool(bulletPrefab, bulletPoolSize);
    }

    private void Update()
    {
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f)
        {
            closestEnemy = FindClosestEnemy();
            if (closestEnemy != null)
            {
                ShootAtEnemy();
                shootTimer = shootInterval;
            }
        }
    }

    private void ShootAtEnemy()
    {
        if (closestEnemy != null)
        {
            Vector3 spawnPos = transform.position + transform.TransformDirection(bulletSpawnOffset);
            Vector3 directionToEnemy = (closestEnemy.position - spawnPos).normalized;
            //directionToEnemy.y = 0; //do stabilizacjlotu pocisku

            Bullet bullet = _bulletPool.GetBullet();
            bullet.transform.position = spawnPos;
            bullet.Initialize(directionToEnemy, _bulletPool);
        }
    }

    private Transform FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float closestDistance = Mathf.Infinity;
        Transform closestTransform = null;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTransform = enemy.transform;
            }
        }
        return closestTransform;
    }
}
