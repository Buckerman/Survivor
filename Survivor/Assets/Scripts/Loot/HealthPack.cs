using Entities.Player;
using UnityEngine;

public class HealthPack : Loot
{
    public float _amount = 5f;
    public override void Initialize(Vector3 position, LootPool pool)
    {
        base.Initialize(position, pool);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController.Instance.GetComponent<PlayerHealth>().Heal(_amount);
            ReturnToPool();
        }
    }
}
