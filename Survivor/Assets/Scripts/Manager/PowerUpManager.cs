using QuangDM.Common;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    private Dictionary<string, PowerUp> activePowerUp;
    private List<string> keyBuffer;

    void Awake()
    {
        activePowerUp = new Dictionary<string, PowerUp>();
        keyBuffer = new List<string>();
    }

    void Update()
    {
        keyBuffer.Clear();
        keyBuffer.AddRange(activePowerUp.Keys);

        foreach (var powerUpName in keyBuffer)
        {
            if (activePowerUp.TryGetValue(powerUpName, out PowerUp powerUp))
            {
                if (powerUp.IsExpired())
                {
                    powerUp.Remove();
                    activePowerUp.Remove(powerUpName);
                    Observer.Instance.Notify(EventName.RemovePowerUpUI, powerUp.powerUpType.ToString());
                    Player.Instance.GetComponent<PlayerAuras>().StopEffect(powerUp.powerUpType);
                }
                else
                {
                    Observer.Instance.Notify(EventName.UpdatePowerUpUI, (powerUp.powerUpType.ToString(), powerUp.TimeRemaining(), powerUp.duration));
                }
            }
        }
    }

    public void AddPowerUp(PowerUp powerUp)
    {
        if (activePowerUp.TryGetValue(powerUp.powerUpType.ToString(), out PowerUp existingPower))
        {
            existingPower.startTime = Time.time;
            existingPower.duration = powerUp.duration;
        }
        else
        {
            activePowerUp[powerUp.powerUpType.ToString()] = powerUp;
            powerUp.Apply();
        }

        Observer.Instance.Notify(EventName.ActivatePowerUpfUI, powerUp.powerUpType.ToString());
        Player.Instance.GetComponent<PlayerAuras>().PlayEffect(powerUp.powerUpType);
    }

    public void ClearAllPowers()
    {
        foreach (var powerUp in activePowerUp.Values)
        {
            powerUp.Remove();
            Player.Instance.GetComponent<PlayerAuras>().StopEffect(powerUp.powerUpType);
        }
        activePowerUp.Clear();
    }
}
