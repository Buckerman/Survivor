using QuangDM.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSpeedBuff : IBuff
{
    public string Name => EventName.MovementSpeedBuff;
    public float Duration { get; set; }
    public float StartTime { get; set; }
    public float Amount { get; set; }

    public MovementSpeedBuff(float duration)
    {
        Duration = duration;
        StartTime = Time.time;
    }

    public void Apply()
    {
        Player.Instance.movementSpeed += 2.0f;
    }

    public void Remove()
    {
        Player.Instance.movementSpeed -= 2.0f;
    }

    public bool IsExpired()
    {
        return Time.time - StartTime >= Duration;
    }

    public float TimeRemaining()
    {
        return Mathf.Max(0, Duration - (Time.time - StartTime));
    }
}
