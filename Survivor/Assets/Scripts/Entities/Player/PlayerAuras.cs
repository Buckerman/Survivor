using UnityEngine;
using QuangDM.Common;

public class PlayerAuras : MonoBehaviour
{
    [SerializeField] public ParticleSystem levelUpAura;
    [SerializeField] public ParticleSystem healAura;
    [SerializeField] public ParticleSystem speedBuffAura;
    [SerializeField] public ParticleSystem damageBuffAura;
    [SerializeField] public ParticleSystem shootingBuffAura;

    private void Start()
    {
        Observer.Instance.AddObserver(EventName.PlayerLevelUp, PlayerLevelUp);
    }

    private void PlayerLevelUp(object data)
    {
        levelUpAura.Play();
    }

    public void PlayEffect(BuffType buffType)
    {
        switch (buffType)
        {
            case BuffType.MOVEMENT_SPEED:
                speedBuffAura.Play();
                break;
            case BuffType.SHOOTING_SPEED:
                shootingBuffAura.Play();
                break;
            case BuffType.MELEE_DAMAGE:
                damageBuffAura.Play();
                break;
        }
    }

    public void StopEffect(BuffType buffType)
    {
        switch (buffType)
        {
            case BuffType.MOVEMENT_SPEED:
                speedBuffAura.Stop();
                break;
            case BuffType.SHOOTING_SPEED:
                shootingBuffAura.Stop();
                break;
            case BuffType.MELEE_DAMAGE:
                damageBuffAura.Stop();
                break;
        }
    }
}