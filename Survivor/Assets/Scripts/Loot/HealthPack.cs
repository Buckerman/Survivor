using UnityEngine;

public class HealthPack : Loot
{
    public override void Initialize(Vector3 position, LootPool pool)
    {
        base.Initialize(position, pool);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Increase player's health
            ReturnToPool();
        }
    }
}
