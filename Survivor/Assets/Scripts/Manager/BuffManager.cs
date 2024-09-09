using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    private Dictionary<string, IBuff> activeBuffs;
    private List<string> keyBuffer;

    void Start()
    {
        activeBuffs = new Dictionary<string, IBuff>();
        keyBuffer = new List<string>();
    }
    void Update()
    {
        keyBuffer.Clear();
        keyBuffer.AddRange(activeBuffs.Keys);

        foreach (var buffName in keyBuffer)
        {
            if (activeBuffs.TryGetValue(buffName, out IBuff buff))
            {
                if (buff.IsExpired())
                {
                    Debug.Log($"Buff {buff.Name} has expired.");
                    buff.Remove();
                    activeBuffs.Remove(buffName);
                }
                else
                {
                    Debug.Log($"Buff {buff.Name} has {buff.TimeRemaining()} seconds remaining.");
                }
            }
        }
    }
    public void AddBuff(IBuff buff)
    {
        if (activeBuffs.TryGetValue(buff.Name, out IBuff existingBuff))
        {
            existingBuff.StartTime = Time.time;
            existingBuff.Duration = buff.Duration;
            Debug.Log($"{buff.Name} timer reset with new duration {buff.Duration} seconds.");
        }
        else
        {
            activeBuffs[buff.Name] = buff;
            buff.Apply();
            Debug.Log($"{buff.Name} added with duration {buff.Duration} seconds.");
        }
    }
}
