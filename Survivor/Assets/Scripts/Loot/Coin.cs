using Entities.Player;
using UnityEngine;
using UnityEngine.Diagnostics;

public class Coin : Loot
{
    private int _amount = 1;
    public override void Initialize(Vector3 position, LootPool pool)
    {
        base.Initialize(position, pool);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController.Instance.GetComponent<PlayerWallet>().UpdateWallet(_amount);
            ReturnToPool();
        }
    }
}
