using DG.Tweening;
using Entities.Player;
using QuangDM.Common;
using System;
using UnityEngine;

public class Coin : Loot
{
    private int _amount = 1;
    public float pickUpRadius = 1.5f;
    public float moveSpeed = 10f;
    private float distanceToPlayer;

    public override void Initialize(Vector3 position, LootPool pool)
    {
        base.Initialize(position, pool);
        Observer.Instance.AddObserver(EventName.PickUpAllLoot, PickUpAllLoot);
    }

    private void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, PlayerController.Instance.transform.position);

        if (distanceToPlayer <= pickUpRadius)
        {
            PickUpLoot();
        }
    }

    private void PickUpLoot()
    {
        transform.position = Vector3.MoveTowards(transform.position, PlayerController.Instance.transform.position, moveSpeed * Time.deltaTime);
        if (distanceToPlayer <= 0.1f)
        {
            PlayerController.Instance.GetComponent<PlayerWallet>().UpdateWallet(_amount);
            Invoke(nameof(RemoveObserver), 0f);
            ReturnToPool();
        }
    }

    private void PickUpAllLoot(object data)
    {
        transform.DOMove(PlayerController.Instance.transform.position, 0.5f).OnComplete(() =>
        {
            Observer.Instance.Notify(EventName.DisableAllLoot);
        });
    }

    private void RemoveObserver()
    {
        Observer.Instance.RemoveObserver(EventName.PickUpAllLoot, PickUpAllLoot);
    }
}