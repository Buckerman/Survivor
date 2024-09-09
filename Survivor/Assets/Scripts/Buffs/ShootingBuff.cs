using QuangDM.Common;
using UnityEngine;

public class ShootingBuff : IBuff
{
    public string Name => EventName.ShootingBuff;
    public float Duration { get; set; }
    public float StartTime { get; set; }

    public ShootingBuff(float duration)
    {
        Duration = duration;
        StartTime = Time.time;
    }

    public void Apply()
    {
        Player.Instance.shootingSpeed *= 1.5f;
    }

    public void Remove()
    {
        Player.Instance.shootingSpeed /= 1.5f;
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
