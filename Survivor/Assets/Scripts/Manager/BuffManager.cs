using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    private Dictionary<string, IBuff> activeBuffs;
    private List<string> keyBuffer;

    void Awake()
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
                    buff.Remove();
                    activeBuffs.Remove(buffName);
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
        }
        else
        {
            activeBuffs[buff.Name] = buff;
            buff.Apply();
        }
    }
    public void ClearAllBuffs()
    {
        foreach (var buff in activeBuffs.Values)
        {
            buff.Remove();
        }
        activeBuffs.Clear();
    }
}
