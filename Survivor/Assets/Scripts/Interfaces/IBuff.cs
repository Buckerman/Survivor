using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuff
{
    string Name { get; }
    float Duration { get; set; }
    float StartTime { get; set; }
    void Apply();
    void Remove();
    bool IsExpired();
    float TimeRemaining();
}

