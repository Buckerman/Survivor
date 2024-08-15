using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private float shootInterval = 2f;
    [SerializeField] private Vector3 bulletSpawnOffset = new Vector3(0.5f, 0, 0.5f);
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private int bulletPoolSize = 10;

    [Header("Right Hand Target")]
    [SerializeField] private ChainIKConstraint rightHandIK;
    [SerializeField] private Transform rightHandTarget;

    private Transform closestEnemy;
    private float shootTimer;
    private BulletPool _bulletPool;
    private Vector3 initialHandRotation = new Vector3(190f, 135f, 90f);

    private void Start()
    {
        _bulletPool = new BulletPool(bulletPrefab, bulletPoolSize);
    }

    private void FixedUpdate()
    {
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f)
        {
            closestEnemy = FindClosestEnemy();
            if (closestEnemy != null)
            {
                rightHandIK.weight = 1f;
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
