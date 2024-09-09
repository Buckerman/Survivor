using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuff
{
    string Name { get; }
    float Duration { get; set; }
    float StartTime { get; set; }
    float Amount { get; set; }//na przyszlosc zeby w player damage calculator
    void Apply();
    void Remove();
    bool IsExpired();
    float TimeRemaining();
}

