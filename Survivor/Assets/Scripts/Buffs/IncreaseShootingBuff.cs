using UnityEngine;

public class IncreaseShootingBuff : IBuff
{
    public string Name => "Faster Shooting";
    public float Duration { get; set; }
    public float StartTime { get; set; }
    private Player playerStats;

    public IncreaseShootingBuff(float duration, Player stats)
    {
        Duration = duration;
        playerStats = stats;
        StartTime = Time.time;
    }

    public void Apply()
    {
        playerStats.shootingSpeed *= 1.5f;
        Debug.Log("Faster Shooting applied.");
    }

    public void Remove()
    {
        playerStats.shootingSpeed /= 1.5f;
        Debug.Log("Faster Shooting removed.");
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
