using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DebuffType
{
    SLOW,
    STUN
}
public class EnemyAuras : MonoBehaviour
{
    [SerializeField] public ParticleSystem freezeAura;
    [SerializeField] public ParticleSystem zapAura;

    public void PlayEffect(DebuffType debuffType)
    {
        switch (debuffType)
        {
            case DebuffType.SLOW:
                freezeAura.Play();
                break;
            case DebuffType.STUN:
                zapAura.Play();
                break;
        }
    }

    public void StopEffect(DebuffType debuffType)
    {
        switch (debuffType)
        {
            case DebuffType.SLOW:
                freezeAura.Stop();
                break;
            case DebuffType.STUN:
                zapAura.Stop();
                break;
        }
    }
}
