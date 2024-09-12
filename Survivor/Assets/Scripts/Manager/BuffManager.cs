using QuangDM.Common;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    private Dictionary<string, PowerUp> activeBuffs;
    private List<string> keyBuffer;

    void Awake()
    {
        activeBuffs = new Dictionary<string, PowerUp>();
        keyBuffer = new List<string>();
    }

    void Update()
    {
        keyBuffer.Clear();
        keyBuffer.AddRange(activeBuffs.Keys);

        foreach (var buffName in keyBuffer)
        {
            if (activeBuffs.TryGetValue(buffName, out PowerUp buff))
            {
                if (buff.IsExpired())
                {
                    buff.Remove();
                    activeBuffs.Remove(buffName);
                    Observer.Instance.Notify(EventName.RemoveBuffUI, buff.buffType.ToString());
                    Player.Instance.GetComponent<PlayerAuras>().StopEffect(buff.buffType);
                }
                else
                {
                    Observer.Instance.Notify(EventName.UpdateBuffUI, (buff.buffType.ToString(), buff.TimeRemaining(), buff.duration));
                }
            }
        }
    }

    public void AddBuff(PowerUp buff)
    {
        if (activeBuffs.TryGetValue(buff.buffType.ToString(), out PowerUp existingBuff))
        {
            existingBuff.startTime = Time.time;
            existingBuff.duration = buff.duration;
        }
        else
        {
            activeBuffs[buff.buffType.ToString()] = buff;
            buff.Apply();
        }

        Observer.Instance.Notify(EventName.ActivateBuffUI, buff.buffType.ToString());
        Player.Instance.GetComponent<PlayerAuras>().PlayEffect(buff.buffType);
    }

    public void ClearAllBuffs()
    {
        foreach (var buff in activeBuffs.Values)
        {
            buff.Remove();
            Player.Instance.GetComponent<PlayerAuras>().StopEffect(buff.buffType);
        }
        activeBuffs.Clear();
    }
}
