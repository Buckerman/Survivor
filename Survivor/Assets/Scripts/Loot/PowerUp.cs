using QuangDM.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuffType
{
    MOVEMENT_SPEED,
    SHOOTING_SPEED,
    MELEE_DAMAGE
}

public class PowerUp : Loot
{
    public float pickUpRadius = 1.5f;
    private float distanceToPlayer;

    [Header("PowerUp Settings")]
    public BuffType buffType;
    public float duration = 10.0f;
    public float amount = 1.5f;
    public float startTime;

    public override void Initialize(Vector3 position)
    {
        base.Initialize(position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            startTime = Time.time;
            GameManager.Instance.GetComponent<BuffManager>().AddBuff(this);
            ReturnToPool();
        }
    }

    public void Apply()
    {
        switch (buffType)
        {
            case BuffType.MOVEMENT_SPEED:
                Player.Instance.movementSpeed += amount;
                break;

            case BuffType.SHOOTING_SPEED:
                Player.Instance.shootingSpeed *= amount;
                break;

            case BuffType.MELEE_DAMAGE:
                Player.Instance.attackDamage += amount;
                break;
        }
    }

    public void Remove()
    {
        switch (buffType)
        {
            case BuffType.MOVEMENT_SPEED:
                Player.Instance.movementSpeed -= amount;
                break;

            case BuffType.SHOOTING_SPEED:
                Player.Instance.shootingSpeed /= amount;
                break;

            case BuffType.MELEE_DAMAGE:
                Player.Instance.attackDamage -= amount;
                break;
        }
    }

    public bool IsExpired()
    {
        return Time.time - startTime >= duration;
    }

    public float TimeRemaining()
    {
        return Mathf.Max(0, duration - (Time.time - startTime));
    }
}
