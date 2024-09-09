using QuangDM.Common;
using UnityEngine;

public class MeleeDamageBuff : IBuff
{
    public string Name => EventName.MeleeDamageBuff;
    public float Duration { get; set; }
    public float StartTime { get; set; }

    public MeleeDamageBuff(float duration)
    {
        Duration = duration;
        StartTime = Time.time;
    }

    public void Apply()
    {
        Player.Instance.attackDamage += 5.0f; 
    }

    public void Remove()
    {
        Player.Instance.attackDamage -= 5.0f; 
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
