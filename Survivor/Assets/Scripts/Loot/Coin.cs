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
            transform.position = Vector3.MoveTowards(transform.position, PlayerController.Instance.transform.position, moveSpeed * Time.deltaTime);
            if (distanceToPlayer <= 0.1f)
            {
                DOTween.Kill(transform);
                PlayerController.Instance.GetComponent<PlayerWallet>().UpdateWallet(_amount);
                Invoke(nameof(RemoveObserver), 0f);
                ReturnToPool();
            }
        }
    }

    private void PickUpAllLoot(object data)
    {
        if (this != null && gameObject.activeInHierarchy)
        {
            transform.DOMove(PlayerController.Instance.transform.position, 0.2f);
        }
    }

    private void RemoveObserver()
    {
        Observer.Instance.RemoveObserver(EventName.PickUpAllLoot, PickUpAllLoot);
    }
}