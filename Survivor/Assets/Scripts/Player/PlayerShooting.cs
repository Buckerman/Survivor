using UnityEngine;
using UnityEngine.Animations.Rigging;
using System.Collections;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private float shootInterval = 2f;
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private int bulletPoolSize = 10;
    public Transform bulletSpawnPos;

    [Header("Right Hand Target")]
    [SerializeField] private ChainIKConstraint rightHandIK;
    [SerializeField] private Transform rightHandTarget;

    private Transform closestEnemy;
    private float shootTimer;
    private BulletPool _bulletPool;

    private void Start()
    {
        _bulletPool = new BulletPool(bulletPrefab, bulletPoolSize);
    }

    private void Update()
    {
        // Adjust the right hand target position each frame based on the player's rotation
        if (rightHandIK.weight > 0f && closestEnemy != null)
        {
            Vector3 directionToEnemy = (closestEnemy.position - transform.position).normalized;
            Vector3 targetOffset = new Vector3(directionToEnemy.x * 3f, 0f, directionToEnemy.z * 3f);
            rightHandTarget.position = transform.position + targetOffset;
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
                StartCoroutine(HandleShooting()); //temp until animator layer learnt
                shootTimer = shootInterval;
            }
        }
    }
    private bool IsEnemyVisible(Transform enemy)
    {
        Renderer enemyRenderer = enemy.GetComponent<Renderer>();

        if (enemyRenderer == null)
            return false;

        return enemyRenderer.isVisible;
    }


    private IEnumerator HandleShooting()//temp until animator layer learnt
    {
        // Smoothly transition to full weight
        yield return StartCoroutine(ChangeIKWeight(1f, 0.2f)); //the duration for the transition

        ShootAtEnemy();

        // Smoothly transition back to zero weight
        yield return StartCoroutine(ChangeIKWeight(0f, 0.2f));
    }

    private IEnumerator ChangeIKWeight(float targetWeight, float duration)//temp until animator layer learnt
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
}
