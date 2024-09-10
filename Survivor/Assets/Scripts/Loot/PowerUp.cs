using QuangDM.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : Loot
{
    public float pickUpRadius = 1.5f;
    private float distanceToPlayer;

    public override void Initialize(Vector3 position, LootPool pool)
    {
        base.Initialize(position, pool);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IBuff buff = new MovementSpeedBuff(10);
            GameManager.Instance.GetComponent<BuffManager>().AddBuff(buff);
        }
    }
}
