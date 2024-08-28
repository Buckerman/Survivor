using UnityEngine;

public class Coin : Loot
{
    public override void Initialize(Vector3 position, LootPool pool)
    {
        base.Initialize(position, pool);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Add coins to player's inventory or score
            ReturnToPool();
        }
    }
}
