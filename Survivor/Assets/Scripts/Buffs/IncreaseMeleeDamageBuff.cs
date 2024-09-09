using UnityEngine;

public class IncreaseMeleeDamageBuff : IBuff
{
    public string Name => "More Melee Damage";
    public float Duration { get; set; }
    public float StartTime { get; set; }
    private Player playerStats;

    public IncreaseMeleeDamageBuff(float duration, Player stats)
    {
        Duration = duration;
        playerStats = stats;
        StartTime = Time.time;
    }

    public void Apply()
    {
        playerStats.attackDamage += 5.0f; 
        Debug.Log("More Melee Damage applied.");
    }

    public void Remove()
    {
        playerStats.attackDamage -= 5.0f; 
        Debug.Log("More Melee Damage removed.");
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
