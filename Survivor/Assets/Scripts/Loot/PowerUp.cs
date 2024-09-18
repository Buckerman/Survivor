using QuangDM.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerUpType
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
    public PowerUpType powerUpType;
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
            GameManager.Instance.GetComponent<PowerUpManager>().AddPowerUp(this);
            ReturnToPool();
        }
    }

    public void Apply()
    {
        switch (powerUpType)
        {
            case PowerUpType.MOVEMENT_SPEED:
                Player.Instance.movementSpeed += amount;
                break;

            case PowerUpType.SHOOTING_SPEED:
                Player.Instance.shootingSpeed *= amount;
                break;

            case PowerUpType.MELEE_DAMAGE:
                Player.Instance.attackDamage += amount;
                break;
        }
    }

    public void Remove()
    {
        switch (powerUpType)
        {
            case PowerUpType.MOVEMENT_SPEED:
                Player.Instance.movementSpeed -= amount;
                break;

            case PowerUpType.SHOOTING_SPEED:
                Player.Instance.shootingSpeed /= amount;
                break;

            case PowerUpType.MELEE_DAMAGE:
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
