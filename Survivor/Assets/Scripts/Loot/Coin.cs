using Entities.Player;
using QuangDM.Common;
using System;
using UnityEngine;

public class Coin : Loot
{
    private int _amount = 1;
    public float pickUpRadius = 1.5f;
    public float moveSpeed = 5f;

    public override void Initialize(Vector3 position, LootPool pool)
    {
        base.Initialize(position, pool);
        Observer.Instance.AddObserver(EventName.PickUpAllLoot, PickUpAllLoot);
    }

    private void Update()
    {
        if (PlayerController.Instance != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, PlayerController.Instance.transform.position);

            if (distanceToPlayer <= pickUpRadius)
            {
                PickUpLoot();
            }
        }
    }

    private void PickUpLoot()
    {
        transform.position = Vector3.MoveTowards(transform.position, PlayerController.Instance.transform.position, moveSpeed * Time.deltaTime);

        PlayerController.Instance.GetComponent<PlayerWallet>().UpdateWallet(_amount);
        Invoke(nameof(RemoveObserver), 0f);
        ReturnToPool();
    }

    private void PickUpAllLoot(object data)
    {
        PickUpLoot();
    }
    private void RemoveObserver()
    {
        Observer.Instance.RemoveObserver(EventName.PickUpAllLoot, PickUpAllLoot);
    }
}
