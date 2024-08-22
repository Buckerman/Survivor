using UnityEngine;
using UnityEngine.Animations.Rigging;
using System.Collections;

public class PlayerShooting : MonoBehaviour
{
    [Header("Shooting Settings")]
    [SerializeField] private float shootInterval = 2f;
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private int bulletPoolSize = 10;
    public Transform bulletSpawnPos;

    [Header("Right Hand Target")]
    [SerializeField] private ChainIKConstraint rightHandIK;
    [SerializeField] private Transform rightHandTarget;

    private Animator animator;
    private Transform closestEnemy;
    private float shootTimer;
    private BulletPool _bulletPool;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
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
            if (closestEnemy != null && IsEnemyVisible(closestEnemy))
            {
                AimAtEnemy();
            }
        }
    }

    private void AimAtEnemy()
    {
        Vector3 directionToEnemy = (closestEnemy.position - transform.position).normalized;
        Vector3 targetOffset = new Vector3(directionToEnemy.x * 3f, 1.3f, directionToEnemy.z * 3f);//custom offset for bullet spawn
        rightHandTarget.position = transform.position + targetOffset;
        //rightHandIK.weight = 1f;  

        animator.Play("PlayerShoot");
    }

    private void ChangeWeight()
    {
        rightHandIK.weight = 0f;
    }

    private void ShootAtEnemy()
    {
        if (closestEnemy != null)
        {
            Vector3 directionToEnemy = (closestEnemy.position - transform.position).normalized;
            directionToEnemy.y = 0;

            Vector3 spawnPos = bulletSpawnPos.position;

            Bullet bullet = _bulletPool.GetBullet();
            bullet.transform.position = spawnPos;
            bullet.Initialize(directionToEnemy, _bulletPool);

            shootTimer = shootInterval;
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

    private bool IsEnemyVisible(Transform enemy)
    {
        Renderer enemyRenderer = enemy.GetComponent<Renderer>();
        return enemyRenderer != null && enemyRenderer.isVisible;
    }
}
