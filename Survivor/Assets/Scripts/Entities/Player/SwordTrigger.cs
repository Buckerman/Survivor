using QuangDM.Common;
using System.Collections.Generic;
using UnityEngine;

public class SwordTrigger : MonoBehaviour
{
    private HashSet<EnemyHealth> enemiesInRange = new HashSet<EnemyHealth>();

    private void Update()
    {
        DetectEnemiesUsingCone(Player.Instance.attackRange);

        if (Player.Instance.GetComponent<PlayerAttack>().isAttacking)
            RemoveEnemiesOutOfRange(Player.Instance.attackRange);
    }

    private void DetectEnemiesUsingCone(float range)
    {
        HashSet<EnemyHealth> detectedEnemies = new HashSet<EnemyHealth>();

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, range);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                EnemyHealth enemyHealth = hitCollider.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    Vector3 directionToEnemy = (hitCollider.transform.position - transform.position).normalized;
                    float distanceToEnemy = Vector3.Distance(transform.position, hitCollider.transform.position);
                    float angle = Vector3.Angle(transform.forward, directionToEnemy);

                    if (angle <= Player.Instance.detectionAngle / 2 && distanceToEnemy <= range)
                    {
                        detectedEnemies.Add(enemyHealth);
                    }
                }
            }
        }

        enemiesInRange = detectedEnemies;

        if (enemiesInRange.Count > 0)
        {
            Observer.Instance.Notify(EventName.Slash, this);
        }
    }

    private void RemoveEnemiesOutOfRange(float range)
    {
        enemiesInRange.RemoveWhere(enemy =>
        {
            if (enemy == null || !enemy.gameObject.activeInHierarchy) return true;

            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            return distance > range;
        });
    }

    public List<EnemyHealth> GetEnemiesInRange()
    {
        return new List<EnemyHealth>(enemiesInRange);
    }

}
