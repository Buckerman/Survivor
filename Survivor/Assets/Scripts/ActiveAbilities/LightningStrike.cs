using UnityEngine;

public class LightningStrike: MonoBehaviour
{
    public float damageAmount = 1f;
    public float radius = 1.5f;
    public float cooldown = 10f;

    void SpawnLightningBolt(Vector3 playerPosition, Vector3 enemyPosition, GameObject lightningBoltPrefab)
    {
        GameObject lightningBoltObject = ObjectPooling.Instance.GetObject(lightningBoltPrefab);
        lightningBoltObject.transform.position = playerPosition;

        Vector3 direction = (enemyPosition - playerPosition).normalized;

        lightningBoltObject.transform.forward = direction;

    }

    // Depending on lengh from enemy, particle system render lengh change
    public void PerformAction(Vector3 playerPosition, GameObject lightningBoltPrefab)
    {
        Collider[] hitColliders = Physics.OverlapSphere(playerPosition, Player.Instance.shootingRange, LayerMask.GetMask("Enemy"));

        foreach (Collider hitCollider in hitColliders)
        {
            float distance = Vector3.Distance(playerPosition, hitCollider.transform.position);
            if (distance <= radius)
            {
                SpawnLightningBolt(playerPosition, hitCollider.transform.position, lightningBoltPrefab);
                hitCollider.gameObject.GetComponent<EnemyHealth>().TakeDamage(damageAmount);
            }
        }
    }
}