using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private float shootInterval = 1f;
    [SerializeField] private Vector3 bulletSpawnOffset = new Vector3(0.5f, 0, 0.5f);

    private Transform closestEnemy;
    private float shootTimer;

    private void Update()
    {
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f)
        {
            closestEnemy = FindClosestEnemy();
            if (closestEnemy != null)
            {
                RotateTowardsEnemy();
                ShootAtEnemy();
                shootTimer = shootInterval;
            }
        }
    }

    private void RotateTowardsEnemy()
    {
        Vector3 directionToEnemy = closestEnemy.position - transform.position;
        directionToEnemy.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(directionToEnemy);
        transform.rotation = targetRotation;
    }

    private void ShootAtEnemy()
    {
        if (closestEnemy != null)
        {
            Vector3 spawnPos = transform.position + transform.TransformDirection(bulletSpawnOffset);
            Vector3 directionToEnemy = (closestEnemy.position - spawnPos).normalized;

            Bullet bullet = BulletPool.Instance.GetBullet();
            bullet.transform.position = spawnPos;
            bullet.Initialize(directionToEnemy);
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
