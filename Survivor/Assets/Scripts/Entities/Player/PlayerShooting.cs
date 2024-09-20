using UnityEngine;
using UnityEngine.Animations.Rigging;
using System.Collections;

public class PlayerShooting : MonoBehaviour
{
    [Header("Shooting Settings")]
    [SerializeField] private GameObject bulletPrefab;
    public Transform bulletSpawnPos;

    [Header("Right Hand Target")]
    [SerializeField] private ChainIKConstraint rightHandIK;
    [SerializeField] private Transform rightHandTarget;

    [Header("Height Tolerance")]
    [SerializeField] private float heightTolerance = 0.5f; // Tolerance for height comparison

    private Animator animator;
    private Transform closestEnemy;
    private float shootTimer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f)
        {
            closestEnemy = FindClosestEnemy();
            if (closestEnemy != null && IsEnemyVisible(closestEnemy) && IsSameHeight(closestEnemy))
            {
                AimAtEnemy();
            }
        }
    }

    private void AimAtEnemy()
    {
        Vector3 directionToEnemy = (closestEnemy.position - transform.position).normalized;
        Vector3 targetOffset = new Vector3(directionToEnemy.x * 3f, 1.3f, directionToEnemy.z * 3f); // Custom offset for bullet spawn
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
            Vector3 directionToEnemy = (closestEnemy.position - transform.position);
            directionToEnemy.y = 0;

            if (directionToEnemy.magnitude < 0.5f)
                directionToEnemy = transform.forward;
            else
                directionToEnemy.Normalize();

            Vector3 spawnPos = bulletSpawnPos.position;

            GameObject bulletObject = ObjectPooling.Instance.GetObject(bulletPrefab);

            Bullet bullet = bulletObject.GetComponent<Bullet>();
            bullet.transform.position = spawnPos;
            bullet.Initialize(directionToEnemy);

            shootTimer = 1f / Player.Instance.shootingSpeed;
        }
    }

    private Transform FindClosestEnemy()
    {
        Vector3 currentPosition = transform.position;

        Collider[] hitColliders = Physics.OverlapSphere(currentPosition, Player.Instance.shootingRange, LayerMask.GetMask("Enemy"));
        float closestDistance = Mathf.Infinity;
        Transform closestTransform = null;

        foreach (Collider hitCollider in hitColliders)
        {
            float distance = Vector3.Distance(currentPosition, hitCollider.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTransform = hitCollider.transform;
            }
        }

        return closestTransform;
    }

    private bool IsEnemyVisible(Transform enemy)
    {
        Renderer enemyRenderer = enemy.GetComponent<Renderer>();
        return enemyRenderer != null && enemyRenderer.isVisible;
    }

    private bool IsSameHeight(Transform enemy)
    {
        float heightDifference = Mathf.Abs(transform.position.y - enemy.position.y);
        return heightDifference <= heightTolerance;
    }
}
