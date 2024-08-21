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

    private void Start()
    {
        _bulletPool = new BulletPool(bulletPrefab, bulletPoolSize);
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (closestEnemy != null && animator.GetLayerWeight(2) > 0f) 
        {
            Vector3 directionToEnemy = (closestEnemy.position - transform.position).normalized;
            Vector3 targetOffset = new Vector3(directionToEnemy.x * 3f, 1.5f, directionToEnemy.z * 3f);
            rightHandTarget.position = transform.position + targetOffset;
            rightHandIK.weight = animator.GetLayerWeight(2);
        }
        else
        {
            rightHandIK.weight = 0f;
        }
    }

    private void FixedUpdate()
    {
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f)
        {
            closestEnemy = FindClosestEnemy();
            if (closestEnemy != null && IsEnemyVisible(closestEnemy))
            {
                StartCoroutine(HandleShooting());
                shootTimer = shootInterval;
            }
        }
    }

    private IEnumerator HandleShooting()
    {
        yield return StartCoroutine(ChangeIKWeight(1f, 0.2f));
        ShootAtEnemy();
        yield return StartCoroutine(ChangeIKWeight(0f, 0.2f));
    }

    private IEnumerator ChangeIKWeight(float targetWeight, float duration)
    {
        float startWeight = rightHandIK.weight;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            rightHandIK.weight = Mathf.Lerp(startWeight, targetWeight, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rightHandIK.weight = targetWeight;
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
